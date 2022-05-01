using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class DeleteChecklistRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}
