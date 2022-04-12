using Microsoft.EntityFrameworkCore;
using Plonks.Boards.Entities;
using Plonks.Boards.Helpers;
using Plonks.Boards.Models;

namespace Plonks.Boards.Services
{
    public interface IBoardService
    {
        Task<BoardResponse<Guid>> AddBoard(AddBoardRequest model);
        Task<BoardResponse<List<BoardDTO>>> GetAllUserBoards(Guid userId);
        Task<BoardResponse<BoardDTO>> GetBoard(GetBoardRequest model);
        Task<BoardResponse<Guid>> FavoriteBoard(FavoriteBoardRequest model);
    }

    public class BoardService : IBoardService
    {

        private readonly AppDbContext _context;

        public BoardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BoardResponse<Guid>> AddBoard(AddBoardRequest model)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(model.UserId));

            if (user == null)
            {
                return new BoardResponse<Guid>() { Message = "No user found." };
            }

            Board board = new Board(model.Title, model.Color, model.UserId);
            BoardUsers boardUsers = new BoardUsers() { BoardId = board.Id, UserId = model.UserId, Favorited = false };

            await _context.Boards.AddAsync(board);
            await _context.BoardUsers.AddAsync(boardUsers);
            await _context.SaveChangesAsync();

            return new BoardResponse<Guid>() { Data = board.Id, Message = "Board added." };
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

        public async Task<BoardResponse<BoardDTO>> GetBoard(GetBoardRequest model)
        {
            BoardUsers boardUser = await _context.BoardUsers.Include(bu => bu.Board).ThenInclude(b => b.Members).FirstOrDefaultAsync((bu) => bu.UserId.Equals(model.UserId) && bu.BoardId.Equals(model.BoardId));

            if (boardUser == null)
            {
                return new BoardResponse<BoardDTO>() { Message = "No board found." };
            }

            List<UserDTO> boardMembers = new List<UserDTO>();

            foreach (User user in boardUser.Board.Members)
            {
                boardMembers.Add(new UserDTO(user, boardUser.Board.OwnerId.Equals(user.Id)));
            }

            return new BoardResponse<BoardDTO>() { Data = new BoardDTO(boardUser.Board, boardUser.Favorited, boardMembers) };
        }

        public async Task<BoardResponse<Guid>> FavoriteBoard(FavoriteBoardRequest model)
        {
            BoardUsers boardUsers = await _context.BoardUsers.FirstOrDefaultAsync(bu => bu.UserId.Equals(model.UserId) && bu.BoardId.Equals(model.BoardId));

            if (boardUsers == null)
            {
                return new BoardResponse<Guid>() { Message = "No user found." };
            }

            boardUsers.Favorited = model.Favorite;

            await _context.SaveChangesAsync();

            return new BoardResponse<Guid>() { Data = boardUsers.BoardId, Message = "Board added." };
        }
    }
}
