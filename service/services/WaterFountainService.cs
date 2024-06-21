using infrastructure;
using infrastructure.Models;

namespace service.services;

public class WaterFountainService
{
    private readonly WaterfountainRepository _waterfountainRepository;

    public WaterFountainService(
        WaterfountainRepository waterfountainRepository){
        _waterfountainRepository = waterfountainRepository;
    }

    public WaterFountainstate getCurrentWaterFountainstate()
    {
        return _waterfountainRepository.getCurrentWaterFountainstate();
    }

    public void addFountaindata(WaterFountainstateDtoToDB waterFountainstate)
    {
        _waterfountainRepository.setWaterFountainState(waterFountainstate);
    }

    public FountainHistory getFountainHistory(int deviceId)
    {
        FountainHistory fountainHistory = new FountainHistory();
        var historyTimeList = new List<Reading>();
        var historyTimeListReturnObject = new List<Reading>();
        var historyOnTime = _waterfountainRepository.getHistoryOnTime(deviceId);
        
        //takes in the times it has been turned on and off and calculates, how long it has been on
        
        foreach (var onTimes in historyOnTime)
        {
            historyTimeList.Add(new Reading
            {
                value = onTimes.isOn ? 1.0 : 0.0,
                timestamp = onTimes.timestamp
            });
        }
        
        for (int i = 0; i < historyTimeList.Count(); i++)
        {
            //If turned on, start counting time until off time
            if (historyTimeList.Count() > i + 1)
            {
                if (historyTimeList[i].value == 1.0 && historyTimeList[i + 1].value == 0.0)
                {
                    double onTime = calculateTimediff(historyTimeList[i].timestamp, historyTimeList[i + 1].timestamp);
                    historyTimeListReturnObject.Add(new Reading
                    {
                        value = onTime,
                        timestamp = historyTimeList[i].timestamp
                    });
                }
            }

        }
        var historyTemperatures = new List<Reading>();
        var historyOnTemperatures = _waterfountainRepository.GetHistoryTemperature(deviceId);
        //takes in the times it has been turned on and off and calculates, how long it has been on
        foreach (var temperatures in historyOnTemperatures)
        {
            historyTemperatures.Add(new Reading
            {
                value = temperatures.temperatur, 
                timestamp = temperatures.timestamp
            });
        }
        
        fountainHistory.onTimeReadings = calculateTotalDailyTime(historyTimeListReturnObject);
        fountainHistory.tempReadings = calculateDailyAverage(historyTemperatures);
        return fountainHistory;
    }

    private List<Reading> calculateTotalDailyTime(List<Reading> historyTime)
    {
        var totals = historyTime.GroupBy(r => r.timestamp.Date)
            .Select(group => new Reading
            {
                timestamp = group.Key,
                value = group.Sum(r => r.value)
            }).ToList();
        return totals;
    }

    public List<Reading> calculateDailyAverage(List<Reading> data)
    {
        var average = data
            .GroupBy(r => r.timestamp.Date)
            .Select(group => new Reading
            {
                timestamp = group.Key,
                value = group.Average(r => r.value)
            })
            .ToList();

        return average;
    }

    private int calculateTimediff(DateTime firstTimestamp, DateTime secondTimestamp)
    {
        TimeSpan onTime = firstTimestamp - secondTimestamp;
        return onTime.Seconds;
    }


    public string getNameFromId(int deviceId)
    {
        return _waterfountainRepository.getFountainNameFromId(deviceId);
    }
}