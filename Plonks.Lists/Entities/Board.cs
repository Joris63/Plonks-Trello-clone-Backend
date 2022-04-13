using System.ComponentModel.DataAnnotations;

namespace Plonks.Lists.Entities
{
    public class Board
    {
        [Key]
        public Guid Id { get; set; }

        public List<BoardList> Lists { get; set; } = new List<BoardList>();
    }
}
