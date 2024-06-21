using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using api.ClientEventFilters;
using api.helpers;
using api.serverEventModels;
using api.WebSocket;
using Fleck;
using lib;
using service.services;

namespace api.clientEventHandlers;

public class ClientWantsToSignInDto : BaseDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email is not valid.")]
    public string email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password is too short.")]
    public string password { get; set; }
}

[ValidateDataAnnotations]
public class ClientWantsToLogIn : BaseEventHandler<ClientWantsToSignInDto>
{
    private readonly AuthService _authService;
    private readonly TokenService _tokenService;

    public ClientWantsToLogIn(
        AuthService authService,
        TokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    public override Task Handle(ClientWantsToSignInDto request, IWebSocketConnection socket)
    {
        //gets user information from db
        var user = _authService.GetUser(request.email);
        

        //checks password hash
        bool validated = _authService.ValidateHash(request.password!, user.PasswordInfo!);
        if (!validated) throw new AuthenticationException("Wrong credentials!");

        //authenticates and sets user information in state service for later use
        StateService.GetClient(socket.ConnectionInfo.Id).IsAuthenticated = true;
        StateService.GetClient(socket.ConnectionInfo.Id).User = user;

        //sends the JWT token to the client
        socket.SendDto(new ServerAuthenticatesUser
        {
            Jwt = _tokenService.IssueJwt(user.Id)
        });
        return Task.CompletedTask;
    }
}