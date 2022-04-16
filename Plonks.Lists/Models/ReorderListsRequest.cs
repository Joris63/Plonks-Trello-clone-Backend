using System.ComponentModel.DataAnnotations;

namespace Plonks.Lists.Models
{
    public class ReorderListsRequest
    {
        [Required]
        public List<BoardListDTO>? Lists { get; set; }

        public Guid BoardId { get; set; }
    }
}
