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

        public AccountService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public Task LoginAsync(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
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
