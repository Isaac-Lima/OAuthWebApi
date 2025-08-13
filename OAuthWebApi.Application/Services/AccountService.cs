using OAuthWebApi.Application.Abstracts;
using OAuthWebApi.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWebApi.Application.Services
{
    internal class AccountService : IaccountIService
    {
        public Task LoginAsync(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
        }

        public Task RefreshToken(string? refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task RegisterAsync(ResgisterRequest resgisterRequest)
        {
            throw new NotImplementedException();
        }
    }
}
