using Microsoft.EntityFrameworkCore;
using Plonks.Cards.Entities;
using Plonks.Cards.Helpers;
using Plonks.Cards.Models;

namespace Plonks.Cards.Services
{
    public interface ICardService
    {
        Task<CardResponse<CardDTO>> AddCard(AddCardRequest model);
        Task<CardResponse<CardDTO>> GetCard(Guid cardId);
        Task<CardResponse<CardDTO>> EditCard(EditCardRequest model);
        Task<CardResponse<bool>> ReorderCards(ReorderCardsRequest model);
        Task<CardResponse<Guid>> ArchiveCard(ArchiveCardRequest model);
    }

    public class CardService : ICardService
    {
        private readonly AppDbContext _context;

        public CardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CardResponse<CardDTO>> AddCard(AddCardRequest model)
        {
            bool listExists = await _context.Lists.AnyAsync((list) => list.Id.Equals(model.ListId));

            if (!listExists)
            {
                return new CardResponse<CardDTO>() { Message = "List was not found." };
            }

            List<Card> result = await _context.Cards.Where(card => card.ListId.Equals(model.ListId) && !card.Archived).ToListAsync();

            Card card = new Card(model.Title, model.ListId, result.Count);

            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync();

            CardDTO response = DTOConverter.CardToDTO(card);

            return new CardResponse<CardDTO>() { Data = response, Message = "Card added." };
        }

        public async Task<CardResponse<CardDTO>> GetCard(Guid cardId)
        {
            Card? card = await _context.Cards
                .Include(card => card.Checklists)
                .ThenInclude(checklist => checklist.Items)
                .Include(card => card.Users)
                .Include(card => card.Comments)
                .FirstOrDefaultAsync((card) => card.Id.Equals(cardId));

            if (card == null)
            {
                return new CardResponse<CardDTO>() { Message = "Card was not found." };
            }

            CardDTO response = DTOConverter.CardToDTO(card);

            return new CardResponse<CardDTO>() { Data = response };
        }

        public async Task<CardResponse<CardDTO>> EditCard(EditCardRequest model)
        {
            Card card = await _context.Cards.FirstOrDefaultAsync((card) => card.Id.Equals(model.Id));

            if (card == null)
            {
                return new CardResponse<CardDTO>() { Message = "Card was not found." };
            }

            if (model.Title != null)
            {
                card.Title = model.Title;
            }

            if (model.Description != null)
            {
                card.Description = model.Description;
            }

            await _context.SaveChangesAsync();

            CardDTO response = DTOConverter.CardToDTO(card);

            return new CardResponse<CardDTO>() { Data = response };
        }

        public async Task<CardResponse<bool>> ReorderCards(ReorderCardsRequest model)
        {
            List<Card> cards = await _context.Cards.Where(card => (card.ListId.Equals(model.DepartureListId) || card.ListId.Equals(model.DestinationListId)) && !card.Archived).ToListAsync();

            foreach (Card card in cards)
            {
                int newOrder = -1;

                if (card.ListId == model.DepartureListId)
                {
                    newOrder = model.DepartureChildren.Find((c) => c.Id == card.Id).Order;
                }
                else
                {
                    newOrder = model.DestinationChildren.Find((c) => c.Id == card.Id).Order;
                }

                if (newOrder < 0 || String.IsNullOrEmpty(newOrder.ToString()))
                {
                    return new CardResponse<bool> { Data = false, Message = "All cards require a correct order." };
                }

                card.Order = newOrder;
            }

            await _context.SaveChangesAsync();

            return new CardResponse<bool> { Data = true, Message = "card order saved." };
        }

        public async Task<CardResponse<Guid>> ArchiveCard(ArchiveCardRequest model)
        {
            Card? card = await _context.Cards.FirstOrDefaultAsync(card => card.Id.Equals(card.Id));

            if (card == null)
            {
                return new CardResponse<Guid> { Message = "Card was not found." };
            }

            card.Archived = model.Archived;

            await _context.SaveChangesAsync();

            return new CardResponse<Guid> { Data = card.Id, Message = model.Archived ? "Card archived." : "Card sent back to the board." };
        }
    }
}
