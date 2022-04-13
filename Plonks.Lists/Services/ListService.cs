using Microsoft.EntityFrameworkCore;
using Plonks.Lists.Entities;
using Plonks.Lists.Helpers;
using Plonks.Lists.Models;

namespace Plonks.Lists.Services
{
    public interface IListService
    {
        Task<BoardListResponse<Guid>> AddList(AddListRequest model);
        Task<BoardListResponse<List<BoardListDTO>>> GetAllListsByBoardId(Guid boardId);
        Task<BoardListResponse<Guid>> EditList(EditListRequest model);
        Task<BoardListResponse<Guid>> ArchiveList(ArchiveListRequest model);
    }

    public class ListService : IListService
    {
        private readonly AppDbContext _context;

        public ListService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BoardListResponse<Guid>> AddList(AddListRequest model)
        {
            bool boardExists = await _context.Boards.AnyAsync((board) => board.Id.Equals(model.BoardId));

            if (!boardExists)
            {
                return new BoardListResponse<Guid>() { Message = "Board was not found." };
            }

            BoardList list = new BoardList(model.Title, model.BoardId);

            await _context.Lists.AddAsync(list);
            await _context.SaveChangesAsync();

            return new BoardListResponse<Guid>() { Data = list.Id, Message = "List added." };
        }

        public async Task<BoardListResponse<List<BoardListDTO>>> GetAllListsByBoardId(Guid boardId)
        {
            bool boardExists = _context.Boards.Any((board) => board.Id.Equals(boardId));

            if (!boardExists)
            {
                return new BoardListResponse<List<BoardListDTO>>() { Message = "Board was not found." };
            }

            List<BoardList> result = await _context.Lists.Include(list => list.Cards).Where(list => list.BoardId.Equals(boardId)).ToListAsync();

            List<BoardListDTO> lists = new List<BoardListDTO>();

            foreach (BoardList list in result)
            {
                List<CardDTO> cards = new List<CardDTO>();

                foreach (Card card in list.Cards)
                {
                    List<UserDTO> users = new List<UserDTO>();

                    foreach (User user in card.Users)
                    {
                        users.Add(new UserDTO()
                        {
                            Id = user.Id,
                            Username = user.Username,
                            Email = user.Email,
                            PicturePath = user.PicturePath,
                        });
                    }

                    cards.Add(new CardDTO()
                    {
                        Id = card.Id,
                        Title = card.Title,
                        ListId = card.ListId,
                        HasDescription = card.HasDescription,
                        CommentAmount = card.CommentAmount,
                        ChecklistItems = card.ChecklistItems,
                        CompletedChecklistItems = card.CompletedChecklistItems,
                        CreatedAt = card.CreatedAt,
                        Users = users,
                    });
                }

                lists.Add(new BoardListDTO()
                {
                    Id = list.Id,
                    Title = list.Title,
                    BoardId = list.BoardId,
                    Cards = cards,
                });
            }

            return new BoardListResponse<List<BoardListDTO>>() { Data = lists };
        }

        public async Task<BoardListResponse<Guid>> EditList(EditListRequest model)
        {
            BoardList? list = await _context.Lists.FirstOrDefaultAsync(list => list.Id.Equals(model.Id));

            if (list == null)
            {
                return new BoardListResponse<Guid> { Message = "No list was found." };
            }

            list.Title = model.Title;

            await _context.SaveChangesAsync();

            return new BoardListResponse<Guid> { Data = list.Id, Message = "List updated." };
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
