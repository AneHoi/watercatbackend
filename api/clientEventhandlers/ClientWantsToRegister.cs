using System.ComponentModel.DataAnnotations;
using api.ClientEventFilters;
using api.helpers;
using api.serverEventModels;
using api.WebSocket;
using Fleck;
using infrastructure.Models;
using lib;
using service.services;

namespace api.clientEventHandlers;  

public class ClientWantsToRegisterDto : BaseDto
{
    [Required(ErrorMessage = "DeviceId is required")]
    public int deviceId { get; set; }
    
    [Required(ErrorMessage = "Device name is required.")]
    public string deviceName { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email is not valid.")]
    public string email { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password is to short.")]
    public string password { get; set; }
    
    [Required(ErrorMessage = "Username is required.")]
    public string username { get; set; }
}

[ValidateDataAnnotations]
public class ClientWantsToRegister : BaseEventHandler<ClientWantsToRegisterDto>
{
    private readonly AuthService _authService;

    private readonly TokenService _tokenService;
    
    public ClientWantsToRegister(
        AuthService authService,
        TokenService tokenService
        )
    {
        _authService = authService;
        _tokenService = tokenService;
        
    }

    public override Task Handle(ClientWantsToRegisterDto dto, IWebSocketConnection socket)
    {
        //Save the user and password to the db
        EndUser user = _authService.RegisterUser(new UserRegisterDto
        {
            deviceId = dto.deviceId,
            deviceName = dto.deviceName,
            email = dto.email,
            password = dto.password,
            username = dto.username
        });

        //issue token
        var token = _tokenService.IssueJwt(user.Id);

        //add user information and validates user to state service for later use
        StateService.GetClient(socket.ConnectionInfo.Id).IsAuthenticated = true;
        StateService.GetClient(socket.ConnectionInfo.Id).User = user;

        //return JWT to client 
        socket.SendDto(new ServerAuthenticatesUser { Jwt = token });
        
        return Task.CompletedTask;
    }
}

