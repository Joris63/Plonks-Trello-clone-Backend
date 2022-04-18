using Microsoft.EntityFrameworkCore;
using Plonks.Lists.Entities;
using Plonks.Lists.Helpers;
using Plonks.Shared.Entities;

namespace Plonks.Lists.Services
{
    public interface IUserService
    {
        Task RegisterUser(SharedUser user);
        Task UpdateUser(SharedUser user);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegisterUser(SharedUser user)
        {
            if (user == null)
            {
                return;
            }

            User newUser = new User()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PicturePath = user.PicturePath,
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(SharedUser user)
        {
            if (user == null)
            {
                return;
            }

            User? retrievedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(user.Id));

            retrievedUser.Username = user.Username;
            retrievedUser.Email = user.Email;
            retrievedUser.PicturePath = user.PicturePath;

            await _context.SaveChangesAsync();
        }
    }
}
