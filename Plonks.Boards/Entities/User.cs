using System.ComponentModel.DataAnnotations;

namespace Plonks.Boards.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PicturePath { get; set; }

        public ICollection<BoardUsers>? Boards { get; set; }
    }
}
