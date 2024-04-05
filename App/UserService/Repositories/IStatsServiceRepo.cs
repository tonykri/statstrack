using UserService.Dto;

namespace UserService.Repositories;

public interface IStatsServiceRepo
{
    Task<List<UserAccountDto>> GetBusinessStats(UserIdsDto userIds);
}