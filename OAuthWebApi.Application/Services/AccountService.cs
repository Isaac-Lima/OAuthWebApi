using Microsoft.AspNetCore.Identity;
using OAuthWebApi.Application.Abstracts;
using OAuthWebApi.Domain.Entities;
using OAuthWebApi.Domain.Exceptions;
using OAuthWebApi.Domain.Requests;

namespace OAuthWebApi.Application.Services
{
    public class AccountService : IaccountIService
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthTokenProcessor _authTokenProcessor;

        public AccountService(UserManager<User> userManager, IAuthTokenProcessor authTokenProcessor)
        {
            _userManager = userManager;
            _authTokenProcessor = authTokenProcessor;
        }

        public async Task LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                throw new LoginFailedException(loginRequest.Email);
            }

            var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);
            var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();

            var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);

            user.RefreshToken = refreshTokenValue;
            user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

            await _userManager.UpdateAsync(user);

            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
            _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken, refreshTokenExpirationDateInUtc);
        }

        public Task RefreshToken(string? refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterAsync(ResgisterRequest resgisterRequest)
        {
            var userExists = await _userManager.FindByEmailAsync(resgisterRequest.Email) != null;

            if (userExists)
            {
                throw new UserAlreadyExistsException(resgisterRequest.Email);
            }

            var user = User.Create(resgisterRequest.Email, resgisterRequest.FirstName, resgisterRequest.LastName);
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, resgisterRequest.Password);

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded) 
            {
               throw new  RegistrationFailedException(result.Errors.Select(x => x.Description));
            }
        }
    }
}
