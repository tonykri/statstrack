using ReviewService.Dto;

namespace ReviewService.Repositories;

public interface IReviewRepo
{
    List<ReviewDto> GetBusinessReviews(Guid businessId);
    void PostReview(CreateReviewDto review);
    void UpdateReview(UpdateReviewDto review);
    void DeleteReview(Guid reviewId);
}