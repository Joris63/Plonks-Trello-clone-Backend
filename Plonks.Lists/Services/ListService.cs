using Microsoft.EntityFrameworkCore;
using Plonks.Lists.Entities;
using Plonks.Lists.Helpers;
using Plonks.Lists.Models;

namespace Plonks.Lists.Services
{
    public interface IListService
    {
        Task<BoardListResponse<BoardListDTO>> AddList(AddListRequest model);
        Task<BoardListResponse<List<BoardListDTO>>> GetAllListsByBoardId(Guid boardId);
        Task<BoardListResponse<BoardListDTO>> EditList(EditListRequest model);
        Task<BoardListResponse<bool>> ReorderLists(ReorderListsRequest model);
        Task<BoardListResponse<Guid>> ArchiveList(ArchiveListRequest model);
    }

    public class ListService : IListService
    {
        private readonly AppDbContext _context;

        public ListService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BoardListResponse<BoardListDTO>> AddList(AddListRequest model)
        {
            bool boardExists = await _context.Boards.AnyAsync((board) => board.Id.Equals(model.BoardId));

            if (!boardExists)
            {
                return new BoardListResponse<BoardListDTO>() { Message = "Board was not found." };
            }

            List<BoardList> result = await _context.Lists.Where(list => list.BoardId.Equals(model.BoardId) && !list.Archived).ToListAsync();

            BoardList list = new BoardList(model.Title, model.BoardId, result.Count);

            await _context.Lists.AddAsync(list);
            await _context.SaveChangesAsync();

            BoardListDTO response = new BoardListDTO() { Id = list.Id, Title = list.Title, BoardId = list.BoardId, Order = list.Order };

            return new BoardListResponse<BoardListDTO>() { Data = response, Message = "List added." };
        }

        public async Task<BoardListResponse<List<BoardListDTO>>> GetAllListsByBoardId(Guid boardId)
        {
            bool boardExists = _context.Boards.Any((board) => board.Id.Equals(boardId));

            if (!boardExists)
            {
                return new BoardListResponse<List<BoardListDTO>>() { Message = "Board was not found." };
            }

            List<BoardList> result = await _context.Lists.OrderBy(list => list.Order).Include(list => list.Cards).Where(list => list.BoardId.Equals(boardId) && !list.Archived).ToListAsync();

            List<BoardListDTO> lists = DTOConverter.MapBoardListsToDTO(result);

            return new BoardListResponse<List<BoardListDTO>>() { Data = lists };
        }

        public async Task<BoardListResponse<BoardListDTO>> EditList(EditListRequest model)
        {
            BoardList? list = await _context.Lists.FirstOrDefaultAsync(list => list.Id.Equals(model.Id));

            if (list == null)
            {
                return new BoardListResponse<BoardListDTO> { Message = "No list was found." };
            }

            list.Title = model.Title;

            await _context.SaveChangesAsync();

            BoardListDTO response = new BoardListDTO() { Id = list.Id, Title = list.Title, BoardId = list.BoardId, Order = list.Order };

            return new BoardListResponse<BoardListDTO> { Data = response, Message = "List updated." };
        }
        public async Task<BoardListResponse<bool>> ReorderLists(ReorderListsRequest model)
        {
            List<BoardList> lists = await _context.Lists.Where(list => list.BoardId.Equals(model.BoardId) && !list.Archived).ToListAsync();

            foreach(BoardList list in lists)
            {
                int newOrder = model.Lists.Find((l) => l.Id == list.Id).Order;

                if(newOrder < 0 || String.IsNullOrEmpty(newOrder.ToString()))
                {
                    return new BoardListResponse<bool> { Data = false, Message = "All lists require a correct order." };
                }

                list.Order = newOrder;
            }

            await _context.SaveChangesAsync();

            return new BoardListResponse<bool> { Data = true, Message = "List order saved." };
        }

        public async Task<BoardListResponse<Guid>> ArchiveList(ArchiveListRequest model)
        {
            BoardList? list = await _context.Lists.FirstOrDefaultAsync(list => list.Id.Equals(model.Id));

            if (list == null)
            {
                return new BoardListResponse<Guid> { Message = "No list was found." };
            }

            list.Archived = model.Archived;

            await _context.SaveChangesAsync();

            return new BoardListResponse<Guid> { Data = list.Id, Message = model.Archived ? "List archived." : "List sent back to the board." };
        }
    }
}
