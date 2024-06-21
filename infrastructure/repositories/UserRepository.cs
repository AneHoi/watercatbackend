using System.Data.SqlTypes;
using Dapper;
using infrastructure.Models;
using Npgsql;

namespace infrastructure;

//TODO fix to postgress
public class UserRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public UserRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    public void TestDatabaseConnection()
    {
        var connectionString = Utilities.ProperlyFormattedConnectionString;
        try
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection to database succeeded!");
                var query = "SELECT * FROM users";
                var result = connection.ExecuteScalar(query);
                Console.WriteLine("Test query executed successfully: " + result);

                Console.WriteLine("Test query executed successfully: " + result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connection failed: {ex.Message}\n{ex.StackTrace}");
        }
    }

    
    public EndUser Create(UserRegisterDto dto)
    {
        try
        {
            using var connection = _dataSource.OpenConnection();
            // create user in User-tabel and return id
            string insertUserQuery = "INSERT INTO Users (email, username) VALUES (@Email, @username);" +
                                     "SELECT id FROM Users WHERE email = @Email;";
            int userId = connection.ExecuteScalar<int>(insertUserQuery, new { Email = dto.email, username = dto.username});

            // Create device
            string insertUserInfoQuery =
                "INSERT INTO devices (id, devivename, userid) VALUES (@id, @deviceName, @userId);";
            connection.Execute(insertUserInfoQuery,
                new { @id = dto.deviceId, deviceName = dto.deviceName, userId = userId });

            
            // Returns EndUser-object
            return new EndUser
            {
                Id = userId,
                Email = dto.email
            };
        }
        catch (Exception ex)
        {
            throw new SqlTypeException("Could not create user in db.", ex);
        }
    }

    public EndUser GetUserByEmail(string requestEmail)
    {
        try
        {
            // Define the query using joins to fetch all information related to the user
            string query = @"
            SELECT users.id, users.email, users.username
            FROM users
            WHERE Email = @Email";

            using (var connection = _dataSource.OpenConnection())
            {
                // Execute the query using Dapper and retrieve the user information
                var user = connection.QueryFirstOrDefault<EndUser>(query, new { Email = requestEmail });
                return user;
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, maybe log them
            throw new SqlTypeException("Failed to retrieve user by email", ex);
        }
    }

    public bool DoesUserExists(string dtoEmail)
    {
        try
        {
            string query = "SELECT COUNT(*) FROM users WHERE email = @Email";
            using (var connection = _dataSource.OpenConnection())
            {
                int count = connection.ExecuteScalar<int>(query, new { Email = dtoEmail });
                
                // returns true if user exists
                return count > 0;
            }

        }
        catch (Exception ex)
        {
            throw new SqlTypeException("failed to search for user", ex);
        }
    }
    
    public int GetDeviceId(int userId)
    {
        try
        {
            // Define the query using joins to fetch all information related to the user
            string query = @"
            SELECT id
            FROM devices
            WHERE userid = @userId";

            using (var connection = _dataSource.OpenConnection())
            {
                // Execute the query using Dapper and retrieve the user information
                var id = connection.QueryFirstOrDefault<int>(query, new { userId = userId });
                return id;
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, maybe log them
            throw new SqlTypeException("Failed to retrieve user by email", ex);
        }
    }
}

public class Emailform
{
    public string email { get; set; }
}