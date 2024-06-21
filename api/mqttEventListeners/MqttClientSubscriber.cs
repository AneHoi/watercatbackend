using System.Text.Json;
using api.helpers;
using api.serverEventModels;
using api.WebSocket;
using Fleck;
using infrastructure.Models;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;
using service.services;

namespace api.mqttEventListeners;

public class MqttClientSubscriber
{
    private DeviceReadingsService _readingsService;
    private WaterFountainService _waterFountainService;

    public MqttClientSubscriber(DeviceReadingsService readingsService, WaterFountainService waterFountainService)
    {
        _readingsService = readingsService;
        _waterFountainService = waterFountainService;
    }

    public async Task CommunicateWithBroker()
    {
        var mqttFactory = new MqttFactory();
        var mqttClient = mqttFactory.CreateMqttClient();

        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer("mqtt.flespi.io", 1883)
            .WithProtocolVersion(MqttProtocolVersion.V500)
            .WithCredentials(Environment.GetEnvironmentVariable(EnvVarKeys.HD_mqttToken.ToString()),
                "")
            .Build();

        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => f.WithTopic("catfountain/waterstate").WithExactlyOnceQoS())
            .Build();

        await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

        mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                Console.WriteLine("what!??");
                var message = e.ApplicationMessage.ConvertPayloadToString();
                var messageObject = JsonSerializer.Deserialize<DeviceData>(message);

                Console.WriteLine("deserialized");
                //TODO send to specific user if connected
                WaterFountainstateDtoToDB waterFountainstate = _readingsService.CreateReadings(messageObject);
                WaterFountainstate currentState = _waterFountainService.getCurrentWaterFountainstate();
                foreach (var webSocketMetaData in StateService._clients)
                {
                    webSocketMetaData.Value.Connection.SendDto(new ServerSendsCurrentFountainstate
                        {
                            ison = currentState.ison,
                            temperatur = currentState.temperatur,
                            TimeStamp = currentState.TimeStamp
                        }
                    );
                }
            }
            catch (Exception exc)
            {
                throw new ConnectionNotAvailableException("could not get data from Mqtt Broker", exc);
            }
        };
        
        var mqttClient2 = mqttFactory.CreateMqttClient();
        var mqttClientOptions2 = new MqttClientOptionsBuilder()
            .WithTcpServer("mqtt.flespi.io", 1883)
            .WithProtocolVersion(MqttProtocolVersion.V500)
            .WithCredentials(Environment.GetEnvironmentVariable(EnvVarKeys.HD_mqttToken.ToString()),
                "")
            .Build();

        await mqttClient2.ConnectAsync(mqttClientOptions2, CancellationToken.None);

        var mqttSubscribeOptions2 = mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => f.WithTopic("catfountain/error"))
            .Build();

        await mqttClient2.SubscribeAsync(mqttSubscribeOptions2, CancellationToken.None);

        mqttClient2.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                Console.WriteLine("what errors!??");
                var message = e.ApplicationMessage.ConvertPayloadToString();
                var messageObject = JsonSerializer.Deserialize<DeviceData>(message);

                Console.WriteLine("deserialized");
                //TODO send to specific user if connected
                WaterFountainstateDtoToDB waterFountainstate = _readingsService.CreateReadings(messageObject);
                WaterFountainstate currentState = _waterFountainService.getCurrentWaterFountainstate();
                foreach (var webSocketMetaData in StateService._clients)
                {
                    webSocketMetaData.Value.Connection.SendDto(new ServerSendsCurrentFountainstate
                        {
                            ison = currentState.ison,
                            temperatur = currentState.temperatur,
                            TimeStamp = currentState.TimeStamp
                        }
                    );
                }
            }
            catch (Exception exc)
            {
                throw new ConnectionNotAvailableException("could not get data from Mqtt Broker", exc);
            }
        };

        
        
    }

    public static async Task sendRequestToTurnOnFountain(int deviceId, int requestedTime)
    {
        Console.WriteLine("sending request");
        var mqttFactory = new MqttFactory();
        var mqttClient = mqttFactory.CreateMqttClient();

        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer("mqtt.flespi.io", 1883)
            .WithProtocolVersion(MqttProtocolVersion.V500)
            .WithCredentials(Environment.GetEnvironmentVariable(EnvVarKeys.HD_mqttToken.ToString()), "")
            .Build();

        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        var pongMessage = new MqttApplicationMessageBuilder()
            .WithTopic("catfountain/clientrequests/" + deviceId) //todo do we want some confirm? 
            .WithPayload(requestedTime + "")
            .Build();
        await mqttClient.PublishAsync(pongMessage, CancellationToken.None);
    }
}