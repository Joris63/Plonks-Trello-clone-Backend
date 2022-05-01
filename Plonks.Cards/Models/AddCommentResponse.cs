namespace Plonks.Cards.Models
{
    public class AddCommentResponse
    {
        public CommentDTO? Comment { get; set; }

        public int CommentCount { get; set; }

        public string Message { get; set; }
    }
}
