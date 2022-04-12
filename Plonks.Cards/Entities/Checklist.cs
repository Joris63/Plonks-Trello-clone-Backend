using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Entities
{
    public class Checklist
    {
        [Key]
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public Guid CardId { get; set; }

        public Card? Card { get; set; }

        public List<ChecklistItem> Items { get; set; } = new List<ChecklistItem>();
    }
}
