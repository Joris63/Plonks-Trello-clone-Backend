using System.ComponentModel.DataAnnotations;

namespace Plonks.Auth.Models
{
    public class RefreshTokenRequest
    {
        [Required]
        public string? RefreshToken { get; set; }
    }
}
