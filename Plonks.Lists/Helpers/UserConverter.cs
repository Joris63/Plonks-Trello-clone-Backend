using Plonks.Lists.Entities;
using Plonks.Shared.Entities;

namespace Plonks.Lists.Helpers
{
    public static class UserConverter
    {
        public static SharedUser UserToSharedUser(User user)
        {
            return new SharedUser()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PicturePath = user.PicturePath,
            };
        }

        public static User SharedUserToUser(SharedUser user)
        {
            return new User()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PicturePath = user.PicturePath,
            };
        }
    }
}
