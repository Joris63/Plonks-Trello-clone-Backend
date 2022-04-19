using Plonks.Cards.Helpers;

namespace Plonks.Cards.Services
{
    public interface IChecklistService
    {

    }

    public class ChecklistService : IChecklistService
    {
        private readonly AppDbContext _context;

        public ChecklistService(AppDbContext context)
        {
            _context = context;
        }


    }
}
