using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class CompleteChecklistItemRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool Complete { get; set; }
    }
}
