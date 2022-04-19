namespace Plonks.Cards.Models
{
    public class ChecklistDTO
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public int Order { get; set; }

        public Guid CardId { get; set; }

        public List<ChecklistItemDTO> Items { get; set; } = new List<ChecklistItemDTO>();
    }
}