using System.ComponentModel.DataAnnotations;

namespace Plonks.Lists.Models
{
    public class AddListRequest
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public Guid BoardId { get; set; }
    }
}
