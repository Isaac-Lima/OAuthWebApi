using Microsoft.AspNetCore.Identity;
using OAuthWebApi.Application.Abstracts;
using OAuthWebApi.Domain.Entities;
using OAuthWebApi.Domain.Exceptions;
using OAuthWebApi.Domain.Requests;
using System.Security.Claims;

namespace OAuthWebApi.Application.Services
{
    public class AccountService : IaccountIService
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthTokenProcessor _authTokenProcessor;
        private readonly IUserRepository _userRepository;

        public AccountService(UserManager<User> userManager, IAuthTokenProcessor authTokenProcessor, IUserRepository userRepository)
        {
            _userManager = userManager;
            _authTokenProcessor = authTokenProcessor;
            _userRepository = userRepository;
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

        public async Task LoginWithGoogleAsync(ClaimsPrincipal? claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ExternalLoginProviderException("Google", "ClaimsPrincipal está nulo");
            }

            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

            if (email == null)
            {
                throw new ExternalLoginProviderException("Google", "Email está nulo");
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var newUser = new User
                {
                    UserName = email,
                    Email = email,
                    FirstName = claimsPrincipal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty,
                    LastName = claimsPrincipal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser);

                if (!result.Succeeded)
                {
                    throw new ExternalLoginProviderException("Google",
                        $"Não é possível criar usuário: {string.Join(", ",
                            result.Errors.Select(x => x.Description))}");
                }



                user = newUser;
            }

            var info = new UserLoginInfo("Google",
                claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
                "Google");

            var loginResult = await _userManager.AddLoginAsync(user, info);

            if (!loginResult.Succeeded)
            {
                throw new ExternalLoginProviderException("Google",
                    $"Não é possível efetuar login do usuário: {string.Join(", ",
                        loginResult.Errors.Select(x => x.Description))}");
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

        public async Task RefreshToken(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new RefreshTokenException("O token de atualização está faltando.");
            }

            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);

            if (user == null)
            {
                throw new RefreshTokenException("Não é possível recuperar o usuário para o token de atualização");
            }

            if (user.RefreshTokenExpiresAtUtc < DateTime.UtcNow)
            {
                throw new RefreshTokenException("O token de atualização expirou.");
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
