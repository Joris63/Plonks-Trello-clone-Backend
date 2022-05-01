using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Entities
{
    public class Checklist
    {
        [Key]
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public int Order { get; set; }

        public Guid CardId { get; set; }

        public Card? Card { get; set; }

        public List<ChecklistItem> Items { get; set; } = new List<ChecklistItem>();


        public Checklist()
        {

        }

        public Checklist(string title, int order, Guid cardId)
        {
            Id = Guid.NewGuid();
            Title = title;
            Order = order;
            CardId = cardId;
        }
    }
}
