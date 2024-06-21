using infrastructure.Models;
using lib;

namespace api.serverEventModels;

public class ServerSendsHistory : BaseDto
{
    public List<Reading> tempReadings { get; set;}
    public List<Reading> onTimeReadings { get; set; }
}