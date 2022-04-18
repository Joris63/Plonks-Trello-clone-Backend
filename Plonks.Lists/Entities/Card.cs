using System.ComponentModel.DataAnnotations;

namespace Plonks.Lists.Entities
{
    public class Card
    {
        [Key]
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public Guid ListId { get; set; }

        public BoardList? List { get; set; }

        public int Order { get; set; }

        public bool Archived { get; set; }

        public bool HasDescription { get; set; }

        public int? CommentAmount { get; set; }

        public int? ChecklistItems { get; set; }

        public int? CompletedChecklistItems { get; set; }

        public DateTime? CreatedAt { get; set; }

        public List<User>? Users { get; set; } = new List<User>();

    }
}
