using Microsoft.EntityFrameworkCore;
using Plonks.Cards.Entities;
using Plonks.Cards.Helpers;
using Plonks.Shared.Entities;

namespace Plonks.Cards.Services
{
    public interface IListService
    {
        Task SaveList(SharedBoardList user);
    }

    public class ListService : IListService
    {
        private readonly AppDbContext _context;

        public ListService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveList(SharedBoardList list)
        {
            if (list == null)
            {
                return;
            }

            BoardList? retrievedList = await _context.Lists.FirstOrDefaultAsync(u => u.Id.Equals(list.Id));

            if (retrievedList != null)
            {
                retrievedList.Title = list.Title;

                await _context.SaveChangesAsync();
            }
            else
            {
                BoardList newList = new BoardList()
                {
                    Id = list.Id,
                    Title = list.Title,
                };

                await _context.Lists.AddAsync(newList);
                await _context.SaveChangesAsync();
            }
        }
    }
}
