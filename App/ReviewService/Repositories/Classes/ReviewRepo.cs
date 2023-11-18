using ReviewService.Dto;
using ReviewService.Models;
using ReviewService.Utils;

namespace ReviewService.Repositories;

public class ReviewRepo : IReviewRepo
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    public ReviewRepo(DataContext dataContext, ITokenDecoder tokenDecoder)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
    }

    public List<ReviewDto> GetBusinessReviews(Guid businessId)
    {
        try
        {
            var business = dataContext.Businesses.FirstOrDefault(b => b.BusinessId == businessId);
            if(business is null)
                throw new NotFoundException("Business not found");
            
            return dataContext.Reviews.Where(r => r.BusinessId == businessId)
                .Select(r => new ReviewDto{
                    Id = r.Id,
                    Stars = r.Stars,
                    Content = r.Content,
                    LastModified = r.LastModified,
                    Response = r.Response != null? new ReviewDto.ReviewResponse{
                        Content = r.Response.Content,
                        LastModified = r.Response.LastModified
                    }: null
                }).OrderByDescending(r => r.LastModified)
                .ToList();
        }catch(NotFoundException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void PostReview(CreateReviewDto review)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var business = dataContext.Businesses.FirstOrDefault(b => b.BusinessId == review.BusinessId);
            if(business is null)
                throw new NotFoundException("Business not found");
            if(!dataContext.VerifiedOrders.Any(v => v.UserId == userId && v.BusinessId == review.BusinessId))
                throw new NotFoundException("There is no verified order to review");
            if(dataContext.Reviews.Any(r => r.UserId == userId && r.BusinessId == review.BusinessId))
                throw new ReviewResExistsException("You have already posted a review for this business");
            
            Review newReview = new Review{
                UserId = userId,
                Business = business,
                BusinessId = business.BusinessId,
                Stars = review.Stars,
                Content = review.Content
            };
            dataContext.Add(newReview);
            dataContext.SaveChanges();
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void UpdateReview(UpdateReviewDto review)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var storedReview = dataContext.Reviews.FirstOrDefault(r => r.Id == review.ReviewId);
            if(storedReview is null)
                throw new NotFoundException("Review not found");
            if(storedReview.UserId != userId)
                throw new NotAllowedException("Review does not belong to this user");
            
            storedReview.Stars = review.Stars;
            storedReview.Content = review.Content;
            storedReview.LastModified = DateTime.Now;
            dataContext.SaveChanges();
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void DeleteReview(Guid reviewId)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var storedReview = dataContext.Reviews.FirstOrDefault(r => r.Id == reviewId);
            if(storedReview is null)
                throw new NotFoundException("Review not found");
            if(storedReview.UserId != userId)
                throw new NotAllowedException("Review does not belong to this user");
            
            dataContext.Remove(storedReview);
            dataContext.SaveChanges();
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}