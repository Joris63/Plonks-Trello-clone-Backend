using Microsoft.EntityFrameworkCore;
using Plonks.Cards.Entities;
using Plonks.Cards.Helpers;
using Plonks.Shared.Entities;

namespace Plonks.Cards.Services
{
    public interface IListService
    {
        Task CreateList(SharedBoardList list);
        Task UpdateList(SharedBoardList list);
    }

    public class ListService : IListService
    {
        private readonly AppDbContext _context;

        public ListService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateList(SharedBoardList list)
        {
            if (list == null)
            {
                return;
            }

            BoardList newList = new BoardList()
            {
                Id = list.Id,
                Title = list.Title,
            };

            await _context.Lists.AddAsync(newList);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateList(SharedBoardList list)
        {
            if (list == null)
            {
                return;
            }

            BoardList? retrievedList = await _context.Lists.FirstOrDefaultAsync(u => u.Id.Equals(list.Id));

            retrievedList.Title = list.Title;

            await _context.SaveChangesAsync();
        }
    }
}
