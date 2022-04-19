using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class ArchiveCardRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool Archived { get; set; }
    }
}
