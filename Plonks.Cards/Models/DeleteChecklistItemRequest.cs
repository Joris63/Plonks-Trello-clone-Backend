using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class DeleteChecklistItemRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}
