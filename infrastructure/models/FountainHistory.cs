namespace infrastructure.Models;

public class FountainHistory
{
    public List<Reading> tempReadings { get; set; }
    public List<Reading> onTimeReadings { get; set; }
}