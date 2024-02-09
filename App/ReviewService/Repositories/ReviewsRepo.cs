using Microsoft.EntityFrameworkCore;
using ReviewService.Dto;
using ReviewService.Models;

namespace ReviewService.Repositories;

public class ReviewsRepo : IReviewsRepo
{
    private readonly DataContext dataContext;
    public ReviewsRepo(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<ApiResponse<List<ReviewDto>, Exception>> GetBusinessReviews(Guid businessId)
    {
        var business = await dataContext.Businesses.FirstOrDefaultAsync(b => b.BusinessId == businessId);
        if (business is null)
            return new ApiResponse<List<ReviewDto>, Exception>(new Exception(ExceptionMessages.BUSINESS_NOT_FOUND));

        var data = await dataContext.Reviews.Where(r => r.BusinessId == businessId)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                Stars = r.Stars,
                Content = r.Content,
                LastModified = r.LastModified,
                Response = r.Response != null ? new ReviewDto.ReviewResponse
                {
                    Content = r.Response.Content,
                    LastModified = r.Response.LastModified
                } : null
            }).OrderByDescending(r => r.LastModified)
            .ToListAsync();

        return new ApiResponse<List<ReviewDto>, Exception>(data);
    }
}