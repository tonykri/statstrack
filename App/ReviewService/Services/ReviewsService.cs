using Microsoft.EntityFrameworkCore;
using ReviewService.AsymcDataProcessing.MessageBusClient;
using ReviewService.Dto;
using ReviewService.Dto.MessageBus.Send;
using ReviewService.Models;
using ReviewService.Utils;

namespace ReviewService.Services;

public class ReviewsService : IReviewsService
{

    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    private readonly IMessageBusClient messageBusClient;
    public ReviewsService(DataContext dataContext, ITokenDecoder tokenDecoder, IMessageBusClient messageBusClient)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
        this.messageBusClient = messageBusClient;
    }


    private void SendBusinessReviewData(Guid businessId)
    {
        int NoOfReviews = dataContext.Reviews
            .Where(r => r.BusinessId == businessId)
            .Count();
        double stars = dataContext.Reviews
            .Where(r => r.BusinessId == businessId)
            .Select(r => r.Stars)
            .Average();

        messageBusClient.UpdateStars(new BusinessStarsDto()
        {
            BusinessId = businessId,
            Stars = stars,
            Reviews = NoOfReviews
        });
    }

    public async Task<ApiResponse<int, Exception>> PostReview(CreateReviewDto review)
    {
        Guid userId = tokenDecoder.GetUserId();
        string fullname = tokenDecoder.GetName();
        var business = await dataContext.Businesses.FirstOrDefaultAsync(b => b.BusinessId == review.BusinessId);
        if (business is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.BUSINESS_NOT_FOUND));
        if (!dataContext.VerifiedOrders.Any(v => v.UserId == userId && v.BusinessId == review.BusinessId))
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.REVIEW_NOT_FOUND));
        if (dataContext.Reviews.Any(r => r.UserId == userId && r.BusinessId == review.BusinessId))
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.REVIEW_EXISTS));

        Review newReview = new Review
        {
            UserId = userId,
            FullName = fullname,
            Business = business,
            BusinessId = business.BusinessId,
            Stars = review.Stars,
            Content = review.Content
        };
        await dataContext.AddAsync(newReview);
        await dataContext.SaveChangesAsync();

        SendBusinessReviewData(business.BusinessId);
        return new ApiResponse<int, Exception>(0);
    }

    public async Task<ApiResponse<int, Exception>> UpdateReview(UpdateReviewDto review)
    {
        Guid userId = tokenDecoder.GetUserId();
        var storedReview = await dataContext.Reviews.FirstOrDefaultAsync(r => r.Id == review.ReviewId);
        if (storedReview is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.REVIEW_NOT_FOUND));
        if (storedReview.UserId != userId)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.UNAUTHORIZED));

        storedReview.Stars = review.Stars;
        storedReview.Content = review.Content;
        storedReview.LastModified = DateTime.Now;
        await dataContext.SaveChangesAsync();

        SendBusinessReviewData(storedReview.BusinessId);
        return new ApiResponse<int, Exception>(0);
    }

    public async Task<ApiResponse<int, Exception>> DeleteReview(Guid reviewId)
    {
        Guid userId = tokenDecoder.GetUserId();
        var storedReview = await dataContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (storedReview is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.REVIEW_NOT_FOUND));
        if (storedReview.UserId != userId)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.UNAUTHORIZED));

        dataContext.Remove(storedReview);
        await dataContext.SaveChangesAsync();

        SendBusinessReviewData(storedReview.BusinessId);
        return new ApiResponse<int, Exception>(0);
    }
}