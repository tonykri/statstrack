using ReviewService.Dto;

namespace ReviewService.Repositories;

public interface IResponseRepo
{
    void PostResponse(CreateUpdateResponseDto response);
    void UpdateReview(CreateUpdateResponseDto response);
    void DeleteResponse(Guid reviewId);
}