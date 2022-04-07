using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Plonks.Boards.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PicturePath { get; set; }

        public List<Board>? Boards { get; set; } = new List<Board>();
    }
}
