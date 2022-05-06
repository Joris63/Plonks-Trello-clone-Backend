using Plonks.Auth.Models;
using System.ComponentModel.DataAnnotations;

namespace Plonks.Auth.Entities
{
    public class User
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Email { get; set; }

        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        public string? PicturePath { get; set; }

        [Required]
        public bool SocialAccount { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public User()
        {

        }

        public User(string username, string email, byte[] passHash, byte[] passSalt)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            PasswordHash = passHash;
            PasswordSalt = passSalt;
            PicturePath = null;
            SocialAccount = false;
        }

        public User(SocialLoginRequest model)
        {
            Id = Guid.NewGuid();
            Username = model.Username;
            Email = model.Email;
            PicturePath = model.PicturePath;
            SocialAccount = true;
        }
    }
}
