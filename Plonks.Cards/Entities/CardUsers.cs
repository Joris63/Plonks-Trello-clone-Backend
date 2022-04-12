using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Entities
{
    public class CardUsers
    {
        [Required]
        public Guid CardId { get; set; }

        public Card Card { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
