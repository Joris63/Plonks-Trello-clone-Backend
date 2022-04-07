using Plonks.Boards.Entities;

namespace Plonks.Boards.Models
{
    public class BoardDTO
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Color { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool Favorited { get; set; } = false;

        public List<UserDTO> Members { get; set; } = new List<UserDTO>();


        public BoardDTO()
        {

        }

        public BoardDTO(Board board, bool favorited, List<UserDTO> members)
        {
            Id = board.Id;
            Title = board.Title;
            Color = board.Color;
            LastUpdated = board.LastUpdated;
            Favorited = favorited;
            Members = members;
        }
    }
}
