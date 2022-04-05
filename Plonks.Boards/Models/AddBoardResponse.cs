using Plonks.Boards.Entities;

namespace Plonks.Boards.Models
{
    public class AddBoardResponse
    {
        public Guid? BoardId { get; set; }

        public string? Message { get; set; }

        public AddBoardResponse(Guid boardId, string message)
        {
            BoardId = boardId;
            Message = message;
        }
    }
}
