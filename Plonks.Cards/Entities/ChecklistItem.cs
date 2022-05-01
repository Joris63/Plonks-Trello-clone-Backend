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


        public ChecklistItem()
        {

        }

        public ChecklistItem(string content, int order, Guid checklistId)
        {
            Id = Guid.NewGuid();
            Content = content;
            Order = order;
            ChecklistId = checklistId;
            Complete = false;
        }
    }
}
