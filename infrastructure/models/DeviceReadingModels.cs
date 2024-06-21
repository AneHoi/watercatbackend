namespace infrastructure.Models;


public class SensorDto
{
    public double MotorValue { get; set; }
    public double TemperatureValue { get; set; }
    public String TimeStamp { get; set; }
}

public class DeviceReadingsDto
{
    public List<SensorDto> DeviceData { get; set; }
}

public class DeviceData
{
    public int DeviceId { get; set; }
    public DeviceReadingsDto Data { get; set; }
}

public class DeviceWaterData
{
    public int DeviceId { get; set; }
    public string Data { get; set; }
}
/*
{
DeviceId : 1
Data : Object
{
    DeviceData : Array [1]
    [
        0 : Object
    {
        MotorValue : 0
        TemperatureValue : 22.625
        TimeStamp : "2024-06-04 20:52:59"
    }
    ]
}
}
*/

/* Example JSON
 {
"DeviceId": 1,
"Data": "hello"
}
*/