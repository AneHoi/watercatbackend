using System.Data.SqlTypes;
using Dapper;
using infrastructure.Models;
using Npgsql;

namespace infrastructure;

public class WaterfountainRepository
{
    
    private readonly NpgsqlDataSource _dataSource;
    public WaterfountainRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }


    public void setWaterFountainState(WaterFountainstateDtoToDB waterFountainstate)
    {
        try
        {
            string query = @"
                INSERT INTO historystatetable (deviceid, temperatur, ison, timestamp)
                VALUES (@deviceId, @temp, @ison, @timestamp);
                ";
            
            // Execute the query using Dapper and retrieve the user information
            using (var connection = _dataSource.OpenConnection())
            {
                connection.Execute(query, new {deviceId = waterFountainstate.deviceId, temp=waterFountainstate.temperatur, ison = waterFountainstate.ison, timestamp = waterFountainstate.TimeStamp});
            }
            
        }
        catch (Exception ex)
        {
            // Handle exceptions, maybe log them
            throw new SqlTypeException("Failed to save the water fountain state", ex);
        }
    }
    public WaterFountainstate getCurrentWaterFountainstate()
    {
        try
        {
            string query = @"
            SELECT *
            FROM historystatetable
            WHERE deviceid = 1
            ORDER BY timestamp DESC;
            ";
            
            // Execute the query using Dapper and retrieve the user information
            using (var connection = _dataSource.OpenConnection())
            {
                var state = connection.QueryFirstOrDefault<WaterFountainstate>(query);
                return state;
            }
            
        }
        catch (Exception ex)
        {
            // Handle exceptions, maybe log them
            throw new SqlTypeException("Failed to retrieve the water fountains state", ex);
        }
    }
    
    //Gets temperatures from the last week
    public IEnumerable<TemperaturesQuery> GetHistoryTemperature(int deviceId)
    {
        try
        {
            string query = $@"SELECT temperatur, timestamp FROM historystatetable WHERE deviceid = @deviceId ORDER BY timestamp DESC;";
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.Query<TemperaturesQuery>(query, new {deviceId = deviceId});
            }
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Failed to retrieve the water fountains history of temperatures", e);

        }
    }

    //Gets the history of on-time and timestamps of the last week
    public IEnumerable<OnTimeQuery> getHistoryOnTime(int deviceId)
    {
        try
        {
            string query = $@"SELECT ison, timestamp FROM historystatetable WHERE deviceid = @deviceId ORDER BY timestamp DESC;";
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.Query<OnTimeQuery>(query, new {deviceId = deviceId});
            }

        }
        catch (Exception e)
        {
            throw new SqlTypeException("Failed to retrieve the water fountains history of on time", e);

        }
            
    }

    public string getFountainNameFromId(int deviceId)
    {
        try
        {
            string query = $@"SELECT devivename FROM devices WHERE id = @deviceId;";
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.QueryFirstOrDefault<string>(query, new {deviceId = deviceId});
            }

        }
        catch (Exception e)
        {
            throw new SqlTypeException("Failed to retrieve the water fountains name", e);
        }
    }
}