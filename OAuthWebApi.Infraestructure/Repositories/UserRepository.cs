using Microsoft.EntityFrameworkCore;
using OAuthWebApi.Domain.Entities;

namespace OAuthWebApi.Infraestructure.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            return user;
        }
    }
}
