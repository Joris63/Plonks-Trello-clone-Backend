using System.ComponentModel.DataAnnotations;

namespace Plonks.Boards.Entities
{
    public class List
    {
        [Required]
        public Guid Id { get; set; }

        public string? Title { get; set; }

        [Required]
        public Guid? BoardId { get; set; }

        public Board Board { get; set; }
    }
}
