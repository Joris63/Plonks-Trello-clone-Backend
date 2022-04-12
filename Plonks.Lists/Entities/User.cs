using System.ComponentModel.DataAnnotations;

namespace Plonks.Lists.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PicturePath { get; set; }

        public List<Card>? Cards { get; set; } = new List<Card>();
    }
}
