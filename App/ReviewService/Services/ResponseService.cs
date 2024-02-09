using Microsoft.EntityFrameworkCore;
using ReviewService.Dto;
using ReviewService.Models;
using ReviewService.Utils;

namespace ReviewService.Services;

public class ResponseService : IResponseService
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    public ResponseService(DataContext dataContext, ITokenDecoder tokenDecoder)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
    }

    public async Task<ApiResponse<int, Exception>> PostResponse(CreateUpdateResponseDto response)
    {
        Guid userId = tokenDecoder.GetUserId();
        var business = await dataContext.Businesses.FirstOrDefaultAsync(b => b.BusinessId == response.BusinessId);
        if (business is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.BUSINESS_NOT_FOUND));
        if (business.UserId != userId)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.UNAUTHORIZED));
        var review = await dataContext.Reviews.FirstOrDefaultAsync(r => r.Id == response.ReviewId);
        if (review is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.REVIEW_NOT_FOUND));
        if (review.Response is not null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.RESPONSE_EXISTS));

        Response newResponse = new Response
        {
            ReviewId = review.Id,
            Review = review,
            Business = business,
            BusinessId = business.BusinessId,
            Content = response.Content
        };
        await dataContext.AddAsync(newResponse);
        await dataContext.SaveChangesAsync();
        return new ApiResponse<int, Exception>(0);
    }

    public async Task<ApiResponse<int, Exception>> UpdateResponse(CreateUpdateResponseDto response)
    {
        Guid userId = tokenDecoder.GetUserId();
        var business = await dataContext.Businesses.FirstOrDefaultAsync(b => b.BusinessId == response.BusinessId);
        if (business is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.BUSINESS_NOT_FOUND));
        if (business.UserId != userId)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.UNAUTHORIZED));
        var review = await dataContext.Reviews.FirstOrDefaultAsync(r => r.Id == response.ReviewId);
        if (review is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.REVIEW_NOT_FOUND));
        if (review.Response is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.RESPONSE_NOT_FOUND));

        review.Response.Content = response.Content;
        review.Response.LastModified = DateTime.Now;
        await dataContext.SaveChangesAsync();
        return new ApiResponse<int, Exception>(0);
    }

    public async Task<ApiResponse<int, Exception>> DeleteResponse(Guid reviewId)
    {
        Guid userId = tokenDecoder.GetUserId();
        var review = await dataContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (review is null)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.REVIEW_NOT_FOUND));
        if (review.Business.UserId != userId)
            return new ApiResponse<int, Exception>(new Exception(ExceptionMessages.UNAUTHORIZED));

        Response response = await dataContext.Responses.FirstAsync(r => r.ReviewId == reviewId);
        dataContext.Remove(response);
        await dataContext.SaveChangesAsync();
        return new ApiResponse<int, Exception>(0);
    }
}