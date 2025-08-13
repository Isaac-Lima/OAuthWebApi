using OAuthWebApi.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWebApi.Application.Abstracts
{
    internal interface IaccountIService
    {
        Task RegisterAsync(ResgisterRequest resgisterRequest);
        Task LoginAsync(LoginRequest loginRequest);
        Task RefreshToken(string? refreshToken);
    }
}
