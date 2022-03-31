using System.ComponentModel.DataAnnotations;

namespace Plonks.Auth.Models
{
    public class SocialLoginRequest
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public string? PicturePath { get; set; } = "";
    }
}
