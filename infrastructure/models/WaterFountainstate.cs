namespace infrastructure.Models;

public class WaterFountainstate
{
    public bool ison { get; set; }
    public double temperatur { get; set; }
    public DateTime TimeStamp { get; set; }
}
public class WaterFountainstateDtoToDB
{
    public int deviceId { get; set; }
    public bool ison { get; set; }
    public double temperatur { get; set; }
    public String TimeStamp { get; set; }
}