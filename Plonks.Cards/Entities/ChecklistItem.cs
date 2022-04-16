using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Entities
{
    public class ChecklistItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ChecklistId { get; set; }

        public Checklist? Checklist { get; set; }

        public string? Content { get; set; }

        public int Order { get; set; }

        public bool Complete { get; set; }
    }
}
