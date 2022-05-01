using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class AddChecklistRequest
    {
        [Required]
        public Guid CardId { get; set; }

        [Required]
        public string? Title { get; set; }
    }
}
