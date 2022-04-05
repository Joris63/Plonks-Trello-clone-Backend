using Plonks.Boards.Entities;
using Plonks.Boards.Helpers;
using Plonks.Boards.Models;

namespace Plonks.Boards.Services
{
    public interface IBoardService
    {
        Task<AddBoardResponse> AddBoard(AddBoardRequest model);
    }

    public class BoardService :IBoardService
    {

        private readonly AppDbContext _context;

        public BoardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AddBoardResponse> AddBoard(AddBoardRequest model)
        {
            Board board = new Board(model.Title, model.Color, model.UserId);

            BoardUsers boardMember = new BoardUsers(board.Id, model.UserId);     

            await _context.Boards.AddAsync(board);
            await _context.BoardUsers.AddAsync(boardMember);
            await _context.SaveChangesAsync();

            return new AddBoardResponse(board.Id, "Board successfully added.");
        }
    }
}
