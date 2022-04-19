using Microsoft.EntityFrameworkCore;
using Plonks.Lists.Entities;
using Plonks.Lists.Helpers;
using Plonks.Shared.Entities;

namespace Plonks.Lists.Services
{
    public interface ICardService
    {
        Task CreateCard(SharedCard card);
        Task UpdateCard(SharedCard card);
    }

    public class CardService : ICardService
    {
        private readonly AppDbContext _context;

        public CardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateCard(SharedCard card)
        {
            if (card == null)
            {
                return;
            }

            List<User> cardUsers = new List<User>();

            foreach (var user in card.Users)
            {
                cardUsers.Add(DTOConverter.SharedUserToUser(user));
            }

            Card newCard = new Card()
            {
                Id = card.Id,
                Title = card.Title,
                ListId = card.ListId,
                Order = card.Order,
                Archived = card.Archived,
                HasDescription = card.HasDescription,
                CommentAmount = card.CommentAmount,
                ChecklistItems = card.ChecklistItems,
                CompletedChecklistItems = card.CompletedChecklistItems,
                CreatedAt = card.CreatedAt,
                Users = cardUsers,
            };

            await _context.Cards.AddAsync(newCard);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCard(SharedCard card)
        {
            if (card == null)
            {
                return;
            }

            Card? retrievedCard = await _context.Cards.FirstOrDefaultAsync(u => u.Id.Equals(card.Id));

            if(retrievedCard == null)
            {
                return;
            }

            List<User> cardUsers = new List<User>();

            foreach (var user in card.Users)
            {
                cardUsers.Add(DTOConverter.SharedUserToUser(user));
            }

            retrievedCard.Title = card.Title;
            retrievedCard.ListId = card.ListId;
            retrievedCard.Order = card.Order;
            retrievedCard.Archived = card.Archived;
            retrievedCard.HasDescription = card.HasDescription;
            retrievedCard.CommentAmount = card.CommentAmount;
            retrievedCard.ChecklistItems = card.ChecklistItems;
            retrievedCard.CompletedChecklistItems = card.CompletedChecklistItems;
            retrievedCard.Users = cardUsers;

            await _context.SaveChangesAsync();
        }
    }
}
