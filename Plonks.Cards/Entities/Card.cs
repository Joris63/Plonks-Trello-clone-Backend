using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Entities
{
    public class Card
    {
        [Key]
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public Guid ListId { get; set; }

        public BoardList? List { get; set; }

        public string? Description { get; set; }

        public bool Archived { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<User> Users { get; set; } =  new List<User>();

        public List<Checklist> Checklists { get; set; } = new List<Checklist>();

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
