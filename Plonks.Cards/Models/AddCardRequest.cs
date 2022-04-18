using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class AddCardRequest
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public Guid ListId { get; set; }
    }
}
