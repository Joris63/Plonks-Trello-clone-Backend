using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class ReorderChecklistItemsRequest
    {
        [Required]
        public Guid ChecklistId { get; set; }

        [Required]
        public List<ChecklistDTO> ChecklistItems { get; set; } = new List<ChecklistDTO>();
    }
}
