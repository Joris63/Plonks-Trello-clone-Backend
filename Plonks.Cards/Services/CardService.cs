using Microsoft.EntityFrameworkCore;
using Plonks.Cards.Entities;
using Plonks.Cards.Helpers;
using Plonks.Cards.Models;

namespace Plonks.Cards.Services
{
    public interface ICardService
    {
        Task<CardResponse<Card>> AddCard(AddCardRequest model);
    }

    public class CardService : ICardService
    {
        private readonly AppDbContext _context;

        public CardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CardResponse<Card>> AddCard(AddCardRequest model)
        {
            bool listExists = await _context.Lists.AnyAsync((list) => list.Id.Equals(model.ListId));

            if (!listExists)
            {
                return new CardResponse<Card>() { Message = "List was not found." };
            }

            List<Card> result = await _context.Cards.Where(card => card.ListId.Equals(model.ListId) && !card.Archived).ToListAsync();

            Card card = new Card(model.Title, model.ListId, result.Count);

            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync();

            return new CardResponse<Card>() { Data = card, Message = "Card added." };
        }
    }
}
