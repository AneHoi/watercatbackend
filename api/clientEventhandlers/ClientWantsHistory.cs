using api.ClientEventFilters;
using api.helpers;
using api.serverEventModels;
using api.WebSocket;
using Fleck;
using infrastructure.Models;
using lib;
using service.services;

namespace api.clientEventHandlers;

public class ClientWantsHistoryDto : BaseDto{}

[RequireAuthentication]
public class ClientWantsHistory : BaseEventHandler<ClientWantsHistoryDto>
{
    private readonly WaterFountainService _waterFountainService;

    public ClientWantsHistory(WaterFountainService waterFountainService)
    {
        _waterFountainService = waterFountainService;
    }
    public override Task Handle(ClientWantsHistoryDto dto, IWebSocketConnection socket)
    {
        int deviceId = StateService.GetClient(socket.ConnectionInfo.Id).User.DeviceId;
        FountainHistory history = _waterFountainService.getFountainHistory(deviceId);
        
        socket.SendDto(new ServerSendsHistory
        {
            tempReadings = history.tempReadings,
            onTimeReadings = history.onTimeReadings
        });
        return Task.CompletedTask;
    }
}
