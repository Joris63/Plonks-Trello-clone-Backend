using System.ComponentModel.DataAnnotations;

namespace Plonks.Auth.Models
{
    public class ChangePasswordRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string NewPassword { get; set; } 

        [Required]
        public string OldPassword { get; set; }
    }
}
