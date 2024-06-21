using lib;

namespace api.serverEventModels;

public class ServerSendsCurrentFountainstate : BaseDto
{
    public bool ison { get; set; }
    public double temperatur { get; set; }
    public DateTime TimeStamp { get; set; }
}