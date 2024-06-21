namespace infrastructure.Models;

public class OnTimeQuery
{
    public bool isOn { get; set; }
    public DateTime timestamp{ get; set; }
}

public class TemperaturesQuery
{
    public double temperatur { get; set; }
    public DateTime timestamp { get; set; }
}