using System.ComponentModel.DataAnnotations;

namespace Plonks.Boards.Models
{
    public class GetBoardRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid BoardId { get; set; }
    }
}
