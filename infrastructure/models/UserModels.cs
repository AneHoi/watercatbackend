using System.ComponentModel.DataAnnotations;

namespace infrastructure.Models;


/**
 * contains all data relevant for stateService SHOULD NOT BE SEND TO CLIENT!
 */
public class EndUser
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string? Email { get; set; }
    public PasswordHash? PasswordInfo;
    //todo should we have admin and ban rights??
}
/**
 * contains all information from a single user EXCLUDING password
 */
public class FullUserDto
{
    public required int Id { get; set; }
    [Required] public required string FullName { get; set; }
    [Required, EmailAddress] public required string Email { get; set; }
    [Required, Timestamp] public required DateTime Created { get; set; }
}

/**
 * shot user dto for public use in client app
 */
public class ShortUserDto
{
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
}

public class UserRegisterDto
{
    [Required(ErrorMessage = "DeviceId is reqired")]
    public int deviceId { get; set; }
    
    [Required(ErrorMessage = "Device name is required.")]
    public string deviceName { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email is not valid.")]
    public string email { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string password { get; set; }
    
    [Required(ErrorMessage = "Name is required.")]
    public string username { get; set; }
    
}
