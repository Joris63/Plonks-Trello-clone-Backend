using System.ComponentModel.DataAnnotations;

namespace Plonks.Auth.Models
{
    public class RevokeTokenRequest
    {
        [Required]
        public Guid? UserId { get; set; }
    }
}
