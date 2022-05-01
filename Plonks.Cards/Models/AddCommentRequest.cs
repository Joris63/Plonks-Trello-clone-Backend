using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class AddCommentRequest
    {
        [Required]
        public Guid CardId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
