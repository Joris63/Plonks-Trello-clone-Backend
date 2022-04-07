using Microsoft.EntityFrameworkCore;
using Plonks.Boards.Entities;
using Plonks.Boards.Helpers;
using Plonks.Boards.Models;

namespace Plonks.Boards.Services
{
    public interface IBoardService
    {
        Task<BoardResponse<Board>> AddBoard(AddBoardRequest model);
        Task<BoardResponse<List<BoardDTO>>> GetAllUserBoards(Guid userId);
    }

    public class BoardService : IBoardService
    {

        private readonly AppDbContext _context;

        public BoardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BoardResponse<Board>> AddBoard(AddBoardRequest model)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(model.UserId));

            if (user == null)
            {
                return new BoardResponse<Board>() { Message = "No user found." };
            }

            Board board = new Board(model.Title, model.Color, model.UserId);

            user.Boards.Add(board);

            await _context.Boards.AddAsync(board);
            await _context.SaveChangesAsync();

            return new BoardResponse<Board>() { Data = board, Message = "Board added." };
        }

        public async Task<BoardResponse<List<BoardDTO>>> GetAllUserBoards(Guid userId)
        {
            bool userExists = await _context.Users.AnyAsync(u => u.Id.Equals(userId));

            if (!userExists)
            {
                return new BoardResponse<List<BoardDTO>>() { Message = "No user found." };
            }

            List<BoardUsers> result = await _context.BoardUsers.Include(bu => bu.Board).ThenInclude(b => b.Members).Where(bu => bu.UserId.Equals(userId)).ToListAsync();

            List<BoardDTO> boards = new List<BoardDTO>();

            foreach(BoardUsers boardUser in result)
            {
                List<UserDTO> boardMembers = new List<UserDTO>();

                foreach(User user in boardUser.Board.Members)
                {
                    boardMembers.Add(new UserDTO(user, boardUser.Board.OwnerId.Equals(user.Id)));
                }

                boards.Add(new BoardDTO(boardUser.Board, boardUser.Favorited, boardMembers));
            }

            return new BoardResponse<List<BoardDTO>>() { Data = boards };
        }
    }
}
