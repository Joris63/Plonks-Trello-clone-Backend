using Microsoft.EntityFrameworkCore;
using Plonks.Lists.Entities;
using Plonks.Lists.Helpers;
using Plonks.Shared.Entities;

namespace Plonks.Lists.Services
{
    public interface ICardService
    {
        Task SaveCard(SharedCard card);
    }

    public class CardService
    {
        private readonly AppDbContext _context;

        public CardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveCard(SharedCard card)
        {
            if (card == null)
            {
                return;
            }

            Card? retrievedCard = await _context.Cards.FirstOrDefaultAsync(u => u.Id.Equals(card.Id));

            if (retrievedCard != null)
            {
                List<User> cardUsers = new List<User>();

                foreach (var user in card.Users)
                {
                    cardUsers.Add(UserConverter.SharedUserToUser(user));
                }

                retrievedCard.Title = card.Title;
                retrievedCard.ListId = card.ListId;
                retrievedCard.HasDescription = card.HasDescription;
                retrievedCard.CommentAmount = card.CommentAmount;
                retrievedCard.ChecklistItems = card.ChecklistItems;
                retrievedCard.CompletedChecklistItems = card.CompletedChecklistItems;
                retrievedCard.Users = cardUsers;

                await _context.SaveChangesAsync();
            }
            else
            {
                List<User> cardUsers = new List<User>();

                foreach (var user in card.Users)
                {
                    cardUsers.Add(UserConverter.SharedUserToUser(user));
                }

                Card newCard = new Card()
                {
                    Id = card.Id,
                    Title = card.Title,
                    ListId = card.ListId,
                    HasDescription = card.HasDescription,
                    CommentAmount = card.CommentAmount,
                    ChecklistItems = card.ChecklistItems,
                    CompletedChecklistItems= card.CompletedChecklistItems,
                    CreatedAt = card.CreatedAt,
                    Users = cardUsers,
                };

                await _context.Cards.AddAsync(newCard);
                await _context.SaveChangesAsync();
            }
        }
    }
}
