using Microsoft.EntityFrameworkCore;
using Plonks.Lists.Entities;
using Plonks.Lists.Helpers;
using Plonks.Shared.Entities;

namespace Plonks.Lists.Services
{
    public interface IBoardService
    {
        Task CreateBoard(SharedBoard board);
        Task DeleteBoard(SharedBoard board);
    }

    public class BoardService : IBoardService
    {
        private readonly AppDbContext _context;

        public BoardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateBoard(SharedBoard board)
        {
            if(board == null)
            {
                return;
            }

            Board newBoard = new Board()
            {
                Id = board.Id,
            };

            await _context.Boards.AddAsync(newBoard);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBoard(SharedBoard board)
        {
            if (board == null)
            {
                return;
            }

            // Delete board and all its content
        }
    }
}
