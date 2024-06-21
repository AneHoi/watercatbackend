using api.helpers;
using api.mqttEventListeners;
using api.serverEventModels;
using Fleck;
using lib;

namespace api.clientEventHandlers;

public class ClientWantsToTurnOnFountainDto : BaseDto{
    public int requestTime { get; set; }

}
public class ClientWantsToTurnOnFountain : BaseEventHandler<ClientWantsToTurnOnFountainDto>
{
    public override Task Handle(ClientWantsToTurnOnFountainDto dto, IWebSocketConnection socket)
    {
        MqttClientSubscriber.sendRequestToTurnOnFountain(1, dto.requestTime);
        socket.SendDto(new ServerConfirmRequestToTurnOn
        {
            message = "Request has been sent to device"
        });
        return Task.CompletedTask;
    }
}

