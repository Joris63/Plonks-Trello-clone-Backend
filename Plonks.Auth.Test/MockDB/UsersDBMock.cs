using Plonks.Auth.Entities;
using Plonks.Auth.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Plonks.Auth.Test.MockDB
{
    public static class UsersDBMock
    {
        public static void SeedDbOneUser(AppDbContext context)
        {
            CreatePasswordHash("Tester123", out byte[] passwordHash, out byte[] passwordSalt);
            DateTime today = DateTime.Now;
            DateTime expiryDate = today.AddDays(7);

            context.Users.Add(new User()
            {
                Id = Guid.Parse("4861d24e-0af5-4e05-b9ca-0d77f8397ec0"),
                Username = "Test",
                Email = "Test@test.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PicturePath = "",
                SocialAccount = false,
                RefreshToken = "645l5abfRPkTOm3bjj+0zqAMr8ReqQU86KR6uvOciiPU8BJ5s+cxOQ2zK+IqMeb/T5+KPe0Hc52KUatVc1VRzw==",
                RefreshTokenExpiryTime = expiryDate,
            });
            context.SaveChanges();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //Create password hash with salt
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
