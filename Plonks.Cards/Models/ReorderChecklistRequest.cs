using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class ReorderChecklistRequest
    {
        [Required]
        public Guid CardId { get; set; }

        [Required]
        public List<ChecklistDTO> Checklists { get; set; } = new List<ChecklistDTO>();
    }
}
