using Microsoft.EntityFrameworkCore;
using Plonks.Cards.Entities;
using Plonks.Cards.Helpers;
using Plonks.Cards.Models;

namespace Plonks.Cards.Services
{
    public interface ICommentService
    {
        Task<AddCommentResponse> AddComment(AddCommentRequest model);
        Task<CardResponse<CommentDTO>> EditComment(EditCommentRequest model);
        Task<DeleteCommentResponse> DeleteComment(DeleteCommentRequest model);
    }

    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;

        public CommentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AddCommentResponse> AddComment(AddCommentRequest model)
        {
            bool cardExists = await _context.Cards.AnyAsync((card) => card.Id.Equals(model.CardId));

            if (!cardExists)
            {
                return new AddCommentResponse() { Message = "Card was not found." };
            }

            Comment comment = new Comment(model.Message, model.UserId, model.CardId);

            List<Comment> comments = await _context.Comments.Where(i => i.CardId.Equals(model.CardId)).ToListAsync();

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            CommentDTO response = DTOConverter.CommentToDTO(comment);

            return new AddCommentResponse() { Comment = response, CommentCount = comments.Count + 1, Message = "Checklist added." };
        }

        public async Task<CardResponse<CommentDTO>> EditComment(EditCommentRequest model)
        {
            Comment comment = await _context.Comments.FirstOrDefaultAsync((comment) => comment.Id.Equals(model.Id));

            if (comment == null)
            {
                return new CardResponse<CommentDTO>() { Message = "Comment was not found." };
            }

            comment.Message = model.Message;

            await _context.SaveChangesAsync();

            CommentDTO response = DTOConverter.CommentToDTO(comment);

            return new CardResponse<CommentDTO>() { Data = response };
        }

        public async Task<DeleteCommentResponse> DeleteComment(DeleteCommentRequest model)
        {
            Comment comment = await _context.Comments.FirstOrDefaultAsync((comment) => comment.Id.Equals(model.Id));

            if (comment == null)
            {
                return new DeleteCommentResponse { Message = "Item was not found." };
            }

            List<Comment> comments = await _context.Comments.Where(i => i.CardId.Equals(comment.CardId) && !i.Id.Equals(model.Id)).ToListAsync();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return new DeleteCommentResponse { CardId = comment.CardId, CommentCount = comments.Count, Message = "Item deleted." };
        }
    }
}
