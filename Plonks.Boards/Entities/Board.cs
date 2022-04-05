using System.ComponentModel.DataAnnotations;

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

        public ICollection<BoardUsers>? Members { get; set; }

        public Board(string title, string color, Guid ownerId)
        {
            Id = Guid.NewGuid();
            Title = title;
            Color = color;
            LastUpdated = DateTime.Now;
            OwnerId = ownerId;
        }
    }
}
