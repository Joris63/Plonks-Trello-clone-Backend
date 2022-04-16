namespace Plonks.Lists.Models
{
    public class CardDTO
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public Guid ListId { get; set; }

        public int Order { get; set; }

        public bool HasDescription { get; set; }

        public int? CommentAmount { get; set; }

        public int? ChecklistItems { get; set; }

        public int? CompletedChecklistItems { get; set; }

        public DateTime? CreatedAt { get; set; }

        public List<UserDTO>? Users { get; set; } = new List<UserDTO>();
    }
}
