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
                cardUsers.Add(new User()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    PicturePath = user.PicturePath,
                });
            }

            Card newCard = new Card()
            {
                Id = card.Id,
                Title = card.Title,
                ListId = card.ListId,
                Order = card.Order,
                Archived = false,
                HasDescription = false,
                CommentAmount = 0,
                ChecklistItems = 0,
                CompletedChecklistItems = 0,
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

            if (retrievedCard == null)
            {
                return;
            }

            List<User> cardUsers = new List<User>();

            foreach (var user in card.Users)
            {
                cardUsers.Add(new User()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    PicturePath = user.PicturePath,
                });
            }

            if(card.Title != null)
            {
                retrievedCard.Title = card.Title;
            }

            if (card.ListId != Guid.Empty)
            {
                retrievedCard.ListId = card.ListId;
            }

            if (card.Order != -1)
            {
                retrievedCard.Order = card.Order;
            }

            if (card.CommentAmount != -1)
            {
                retrievedCard.CommentAmount = card.CommentAmount;
            }

            if (card.ChecklistItems != -1)
            {
                retrievedCard.ChecklistItems = card.ChecklistItems;
            }

            if (card.CompletedChecklistItems != -1)
            {
                retrievedCard.CompletedChecklistItems = card.CompletedChecklistItems;
            }

            if (card.Users != null)
            {
                retrievedCard.Users = cardUsers;
            }

            retrievedCard.Archived = card.Archived;
            retrievedCard.HasDescription = card.HasDescription;

            await _context.SaveChangesAsync();
        }
    }
}
