using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PicturePath { get; set; }

        public List<Card>? Tasks { get; set; } = new List<Card>();

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
