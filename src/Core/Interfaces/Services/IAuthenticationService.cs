using Core.Models;

namespace Core.Interfaces.Services;

public interface IAuthenticationService
{
    public Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest);
}