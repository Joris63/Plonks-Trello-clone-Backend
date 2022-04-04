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

        public ICollection<User> Users { get; set; }

        public ICollection<List> Lists { get; set; }
    }
}
