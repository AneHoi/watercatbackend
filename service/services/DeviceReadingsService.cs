using System.Data.SqlTypes;
using infrastructure.Models;

namespace service.services;

public class DeviceReadingsService
{
    private readonly WaterFountainService _waterFountainService;
    public DeviceReadingsService(WaterFountainService waterFountainService)
    {
        _waterFountainService = waterFountainService;
    }

    public WaterFountainstateDtoToDB CreateReadings(DeviceData deviceReadingsDto)
    {
        var deviceId = deviceReadingsDto.DeviceId;
        //TODO remove
        Console.WriteLine("recieved");
        //Console.WriteLine("Data: " + deviceReadingsDto.Data.Length);

        if (deviceReadingsDto.Data.DeviceData.Any())
        {
            Console.WriteLine("Data: " + deviceReadingsDto.Data.DeviceData.Count);
            WaterFountainstateDtoToDB waterFountainstate = new WaterFountainstateDtoToDB()
            {
                deviceId = deviceReadingsDto.DeviceId,
                ison = deviceReadingsDto.Data.DeviceData[0].MotorValue == 1 ? true : false,
                temperatur = deviceReadingsDto.Data.DeviceData[0].TemperatureValue,
                TimeStamp = deviceReadingsDto.Data.DeviceData[0].TimeStamp,
            };
            _waterFountainService.addFountaindata(waterFountainstate);
            return waterFountainstate;
        }
        else
            throw new NullReferenceException("There is no correct data");
    }

    public bool DeleteAllReadings(int deviceId)
    {
        /*
         var wasHumidityDeleted = _humidityRepository.DeleteHumidityReadings(deviceId);
        if (!wasHumidityDeleted)
            throw new SqlTypeException("Failed to delete humidity readings");

        var wasTemperatureDeleted = _temperatureRepository.DeleteTemperatureReadings(deviceId);
        if (!wasTemperatureDeleted)
            throw new SqlTypeException("Failed to delete temperature readings");

        var wasParticle25Deleted = _particlesRepository.DeleteParticle25(deviceId);
        if (!wasParticle25Deleted)
            throw new SqlTypeException("Failed to delete particle 2.5 readings");

        var wasParticle100Deleted = _particlesRepository.DeleteParticle100(deviceId);
        if (!wasParticle100Deleted)
            throw new SqlTypeException("Failed to delete particle 10.0 readings");
                 */
        return true;
    }
}