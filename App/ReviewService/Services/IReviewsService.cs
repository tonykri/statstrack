using ReviewService.Dto;

namespace ReviewService.Services;

public interface IReviewsService {
    Task<ApiResponse<int, Exception>> PostReview(CreateReviewDto review);
    Task<ApiResponse<int, Exception>> UpdateReview(UpdateReviewDto review);
    Task<ApiResponse<int, Exception>> DeleteReview(Guid reviewId);
}