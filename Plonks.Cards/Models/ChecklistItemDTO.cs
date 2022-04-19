namespace Plonks.Cards.Models
{
    public class ChecklistItemDTO
    {
        public Guid Id { get; set; }

        public Guid ChecklistId { get; set; }

        public string? Content { get; set; }

        public int Order { get; set; }

        public bool Complete { get; set; }
    }
}