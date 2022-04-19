namespace Plonks.Cards.Models
{
    public class CommentDTO
    {
        public Guid Id { get; set; }

        public string? Message { get; set; }

        public Guid SenderId { get; set; }

        public UserDTO? Sender { get; set; }

        public Guid CardId { get; set; }

        public DateTime SentAt { get; set; }
    }
}