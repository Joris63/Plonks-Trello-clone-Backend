using Plonks.Cards.Entities;
using Plonks.Cards.Models;

namespace Plonks.Cards.Helpers
{
    public static class DTOConverter
    {
        public static CardDTO CardToDTO(Card card)
        {
            return new CardDTO()
            {
                Id = card.Id,
                Title = card.Title,
                ListId = card.ListId,
                ListTitle = card.List?.Title,
                Order = card.Order,
                Description = card.Description,
                Archived = card.Archived,
                CreatedAt = card.CreatedAt,
                Users = MapUsersToDTO(card.Users),
                Checklists = MapChecklistsToDTO(card.Checklists),
                Comments = MapCommentsToDTO(card.Comments)
            };
        }
        public static UserDTO UserToDTO(User user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PicturePath = user.PicturePath,
            };
        }

        public static ChecklistDTO ChecklistToDTO(Checklist checklist)
        {
            return new ChecklistDTO()
            {
                Id = checklist.Id,
                Title = checklist.Title,
                Order = checklist.Order,
                CardId = checklist.CardId,
                Items = MapChecklistItemsToDTO(checklist.Items),
            };
        }

        public static ChecklistItemDTO ChecklistItemToDTO(ChecklistItem item)
        {
            return new ChecklistItemDTO()
            {
                Id = item.Id,
                ChecklistId = item.ChecklistId,
                Content = item.Content,
                Order = item.Order,
                Complete = item.Complete,
            };
        }

        public static CommentDTO CommentToDTO(Comment comment)
        {
            return new CommentDTO()
            {
                Id = comment.Id,
                Message = comment.Message,
                SenderId = comment.SenderId,
                Sender = UserToDTO(comment.Sender),
                CardId = comment.CardId,
                SentAt = comment.SentAt,
            };
        }

        public static List<UserDTO> MapUsersToDTO(List<User> users)
        {
            List<UserDTO> result = new List<UserDTO>();

            foreach (User user in users)
            {
                result.Add(UserToDTO(user));
            }

            return result;
        }

        public static List<ChecklistDTO> MapChecklistsToDTO(List<Checklist> checklists)
        {
            List<ChecklistDTO> result = new List<ChecklistDTO>();

            foreach (Checklist checklist in checklists)
            {
                result.Add(ChecklistToDTO(checklist));
            }

            return result;
        }

        public static List<ChecklistItemDTO> MapChecklistItemsToDTO(List<ChecklistItem> items)
        {
            List<ChecklistItemDTO> result = new List<ChecklistItemDTO>();

            foreach (ChecklistItem item in items)
            {
                result.Add(ChecklistItemToDTO(item));
            }

            return result;
        }

        public static List<CommentDTO> MapCommentsToDTO(List<Comment> comments)
        {
            List<CommentDTO> result = new List<CommentDTO>();

            foreach (Comment comment in comments)
            {
                result.Add(CommentToDTO(comment));
            }

            return result;
        }
    }
}
