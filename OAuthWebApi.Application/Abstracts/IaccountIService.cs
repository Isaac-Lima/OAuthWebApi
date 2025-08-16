using OAuthWebApi.Domain.Requests;
using System.Security.Claims;

namespace OAuthWebApi.Application.Abstracts
{
    public interface IaccountIService
    {
        Task RegisterAsync(ResgisterRequest resgisterRequest);
        Task LoginAsync(LoginRequest loginRequest);
        Task RefreshToken(string? refreshToken);
        Task LoginWithGoogleAsync(ClaimsPrincipal? claimsPrincipal);
    }
}
