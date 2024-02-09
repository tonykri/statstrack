using ReviewService.Dto;

namespace ReviewService.Repositories;

public interface IReviewsRepo
{
    Task<ApiResponse<List<ReviewDto>, Exception>> GetBusinessReviews(Guid businessId);
}