using OAuthWebApi.Domain.Entities;

namespace OAuthWebApi.Application.Abstracts
{
    public interface IUserRepository
    {
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    }
}
