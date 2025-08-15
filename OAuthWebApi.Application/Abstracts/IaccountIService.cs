using OAuthWebApi.Domain.Requests;

namespace OAuthWebApi.Application.Abstracts
{
    public interface IaccountIService
    {
        Task RegisterAsync(ResgisterRequest resgisterRequest);
        Task LoginAsync(LoginRequest loginRequest);
        Task RefreshToken(string? refreshToken);
    }
}
