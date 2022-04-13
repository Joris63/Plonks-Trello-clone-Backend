using System.ComponentModel.DataAnnotations;

namespace Plonks.Lists.Models
{
    public class ArchiveListRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool Archived { get; set; }
    }
}
