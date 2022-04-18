using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Entities
{
    public class BoardList
    {
        [Key]
        public Guid Id { get; set; }

        public List<Card> Cards { get; set; } = new List<Card>();
    }
}
