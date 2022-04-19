using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class EditCardRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
