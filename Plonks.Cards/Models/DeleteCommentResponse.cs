namespace Plonks.Cards.Models
{
    public class DeleteCommentResponse
    {
        public Guid CardId { get; set; }

        public int CommentCount { get; set; }

        public string Message { get; set; }
    }
}
