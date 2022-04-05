using System.ComponentModel.DataAnnotations;

namespace Plonks.Boards.Models
{
    public class AddBoardRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Color {  get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
