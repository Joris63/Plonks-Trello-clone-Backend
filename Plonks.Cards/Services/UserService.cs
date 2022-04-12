using Microsoft.EntityFrameworkCore;
using Plonks.Cards.Entities;
using Plonks.Cards.Helpers;
using Plonks.Shared.Entities;

namespace Plonks.Cards.Services
{
    public interface IUserService
    {
        Task SaveUser(SharedUser user);
    }
         
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveUser(SharedUser user)
        {
            if (user == null)
            {
                return;
            }

            User? retrievedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(user.Id));

            if (retrievedUser != null)
            {
                retrievedUser.Username = user.Username;
                retrievedUser.Email = user.Email;
                retrievedUser.PicturePath = user.PicturePath;

                await _context.SaveChangesAsync();
            }
            else
            {
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
        }
    }
}
