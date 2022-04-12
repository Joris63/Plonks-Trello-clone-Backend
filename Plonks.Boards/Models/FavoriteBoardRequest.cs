using System.ComponentModel.DataAnnotations;

namespace Plonks.Boards.Models
{
    public class FavoriteBoardRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid BoardId { get; set; }

        [Required]
        public bool Favorite { get; set; }
    }
}
