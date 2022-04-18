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

        public int Order { get; set; }

        public string? Description { get; set; }

        public bool Archived { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<User> Users { get; set; } =  new List<User>();

        public List<Checklist> Checklists { get; set; } = new List<Checklist>();

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public Card()
        {

        }

        public Card(string title, Guid listId, int order)
        {
            Id = Guid.NewGuid();
            Title = title;
            ListId = listId;
            Order = order;
            CreatedAt = DateTime.Now;
        }
    }
}
