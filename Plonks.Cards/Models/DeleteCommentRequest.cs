using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class DeleteCommentRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}
