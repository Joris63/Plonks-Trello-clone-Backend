using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        public string? Message { get; set; }

        public Guid SenderId { get; set; }

        public User? Sender { get; set; }

        public Guid CardId { get; set; }

        public Card? Card { get; set; }

        public DateTime SentAt { get; set; }


        public Comment()
        {

        }

        public Comment(string message, Guid senderId, Guid cardId)
        {
            Id = Guid.NewGuid();
            Message = message;
            SenderId = senderId;
            CardId = cardId;
            SentAt = DateTime.UtcNow;
        }
    }
}
