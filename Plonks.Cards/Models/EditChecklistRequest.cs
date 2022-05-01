using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class EditChecklistRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Title { get; set; }
    }
}
