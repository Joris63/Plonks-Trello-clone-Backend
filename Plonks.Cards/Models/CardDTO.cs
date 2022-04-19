namespace Plonks.Cards.Models
{
    public class CardDTO
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public Guid ListId { get; set; }

        public string? ListTitle { get; set; }

        public int Order { get; set; }

        public string? Description { get; set; }

        public bool Archived { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<UserDTO> Users { get; set; } = new List<UserDTO>();

        public List<ChecklistDTO> Checklists { get; set; } = new List<ChecklistDTO>();

        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
    }
}
