using api.ClientEventFilters;
using api.helpers;
using api.serverEventModels;
using api.WebSocket;
using Fleck;
using lib;
using service.services;

namespace api.clientEventHandlers;

public class ClientWantsFountainNameDto : BaseDto
{}

[RequireAuthentication]
public class ClientWantsFountainName : BaseEventHandler<ClientWantsFountainNameDto>
{
    private readonly WaterFountainService _waterFountainService;

    public ClientWantsFountainName(WaterFountainService waterFountainService)
    {
        _waterFountainService = waterFountainService;
    }
    public override Task Handle(ClientWantsFountainNameDto dto, IWebSocketConnection socket)
    {
        int deviceId = StateService.GetClient(socket.ConnectionInfo.Id).User.DeviceId;
        String fountainName = _waterFountainService.getNameFromId(deviceId);
        socket.SendDto(new ServerSendsFountainName
        {
            fountainName = fountainName
        });
        return Task.CompletedTask;
    }
}