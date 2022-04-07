using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plonks.Boards.Entities
{
    public class Board
    {
        [Key]
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Color { get; set; }

        public DateTime LastUpdated { get; set; }

        public Guid OwnerId { get; set; }

        public List<User>? Members { get; set; } = new List<User>();

        public Board()
        {

        }

        public Board(string title, string color, Guid ownerID)
        {
            Id = Guid.NewGuid();
            Title = title;
            Color = color;
            LastUpdated = DateTime.Now;
            OwnerId = ownerID;
        }
    }
}
