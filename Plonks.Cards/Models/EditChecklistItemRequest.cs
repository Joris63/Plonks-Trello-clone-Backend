using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class EditChecklistItemRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Content { get; set; }
    }
}
