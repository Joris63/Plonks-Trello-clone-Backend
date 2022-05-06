using System.ComponentModel.DataAnnotations;

namespace Plonks.Auth.Models
{
    public class EditUserRequest
    {
        [Required]
        public Guid Id { get; set; }

        public string? Username { get; set; } = null;

        [EmailAddress]
        public string? Email { get; set; } = null;

        public string? PicturePath { get; set; } = null;
    }
}
