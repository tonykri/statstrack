using ReviewService.Dto;

namespace ReviewService.Services;

public interface IResponseService {
    Task<ApiResponse<int, Exception>> PostResponse(CreateUpdateResponseDto response);
    Task<ApiResponse<int, Exception>> UpdateResponse(CreateUpdateResponseDto response);
    Task<ApiResponse<int, Exception>> DeleteResponse(Guid reviewId);
}