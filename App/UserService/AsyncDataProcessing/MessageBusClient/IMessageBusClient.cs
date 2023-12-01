using UserService.Dto.MessageBus.Send;

namespace UserService.AsymcDataProcessing.MessageBusClient;

public interface IMessageBusClient
{
    void DeleteUser(UserDeletedDto userDeletedDto);
    void UpdateUser(UserUpdatedDto userUpdatedDto);
}