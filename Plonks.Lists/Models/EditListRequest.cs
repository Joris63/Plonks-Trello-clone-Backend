using System.ComponentModel.DataAnnotations;

namespace Plonks.Lists.Models
{
    public class EditListRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Title { get; set; }
    }
}
