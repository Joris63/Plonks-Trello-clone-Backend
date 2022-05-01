using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class AddChecklistItemRequest
    {
        [Required]
        public Guid ChecklistId { get; set; }

        [Required]
        public string? Content { get; set; }
    }
}
