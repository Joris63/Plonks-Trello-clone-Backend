using Plonks.Lists.Entities;
using Plonks.Lists.Models;
using Plonks.Shared.Entities;

namespace Plonks.Lists.Helpers
{
    public static class DTOConverter
    {
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

        public static BoardListDTO BoardListToDTO(BoardList list)
        {
            return new BoardListDTO()
            {
                Id = list.Id,
                Title = list.Title,
                BoardId = list.BoardId,
                Order = list.Order,
                Cards = MapCardsToDTO(list.Cards)
            };
        }

        public static CardDTO CardToDTO(Card card)
        {
            return new CardDTO()
            {
                Id = card.Id,
                Title = card.Title,
                ListId = card.ListId,
                Order = card.Order,
                HasDescription = card.HasDescription,
                CommentAmount = card.CommentAmount,
                ChecklistItems = card.ChecklistItems,
                CompletedChecklistItems = card.CompletedChecklistItems,
                CreatedAt = card.CreatedAt,
                Users = MapUsersToDTO(card.Users)
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
        public static List<BoardListDTO> MapBoardListsToDTO(List<BoardList> lists)
        {
            List<BoardListDTO> result = new List<BoardListDTO>();

            foreach (BoardList list in lists)
            {
                result.Add(BoardListToDTO(list));
            }

            return result;
        }

        public static List<CardDTO> MapCardsToDTO(List<Card> cards)
        {
            List<CardDTO> result = new List<CardDTO>();

            foreach (Card card in cards)
            {
                result.Add(CardToDTO(card));
            }

            return result;
        }
    }
}
