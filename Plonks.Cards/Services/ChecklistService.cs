using Microsoft.EntityFrameworkCore;
using Plonks.Cards.Entities;
using Plonks.Cards.Helpers;
using Plonks.Cards.Models;

namespace Plonks.Cards.Services
{
    public interface IChecklistService
    {
        Task<CardResponse<ChecklistDTO>> AddChecklist(AddChecklistRequest model);
        Task<AddChecklistItemResponse> AddChecklistItem(AddChecklistItemRequest model);
        Task<CardResponse<ChecklistDTO>> EditChecklist(EditChecklistRequest model);
        Task<CardResponse<ChecklistItemDTO>> EditChecklistItem(EditChecklistItemRequest model);
        Task<CardResponse<int>> CompleteChecklistItem(CompleteChecklistItemRequest model);
        Task<CardResponse<bool>> ReorderChecklists(ReorderChecklistRequest model);
        Task<CardResponse<bool>> ReorderChecklistItems(ReorderChecklistItemsRequest model);
        Task<CardResponse<Guid>> DeleteChecklist(DeleteChecklistRequest model);
        Task<DeleteChecklistItemResponse> DeleteChecklistItem(DeleteChecklistItemRequest model);
    }

    public class ChecklistService : IChecklistService
    {
        private readonly AppDbContext _context;

        public ChecklistService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CardResponse<ChecklistDTO>> AddChecklist(AddChecklistRequest model)
        {
            bool cardExists = await _context.Cards.AnyAsync((card) => card.Id.Equals(model.CardId));

            if (!cardExists)
            {
                return new CardResponse<ChecklistDTO>() { Message = "Card was not found." };
            }

            List<Checklist> result = await _context.Checklists.Where(checklist => checklist.CardId.Equals(model.CardId)).ToListAsync();

            Checklist checklist = new Checklist(model.Title, result.Count, model.CardId);

            await _context.Checklists.AddAsync(checklist);
            await _context.SaveChangesAsync();

            ChecklistDTO response = DTOConverter.ChecklistToDTO(checklist);

            return new CardResponse<ChecklistDTO>() { Data = response, Message = "Checklist added." };
        }

        public async Task<AddChecklistItemResponse> AddChecklistItem(AddChecklistItemRequest model)
        {
            bool checklistExists = await _context.Checklists.AnyAsync((checklist) => checklist.Id.Equals(model.ChecklistId));

            if (!checklistExists)
            {
                return new AddChecklistItemResponse() { Message = "Checklist was not found." };
            }

            List<ChecklistItem> result = await _context.ChecklistItems.Where(item => item.ChecklistId.Equals(model.ChecklistId)).ToListAsync();

            ChecklistItem item = new ChecklistItem(model.Content, result.Count, model.ChecklistId);

            await _context.ChecklistItems.AddAsync(item);
            await _context.SaveChangesAsync();

            ChecklistItemDTO response = DTOConverter.ChecklistItemToDTO(item);

            return new AddChecklistItemResponse() { ChecklistItem = response, ChecklistItemsCount = result.Count + 1, Message = "Item added." };
        }

        public async Task<CardResponse<ChecklistDTO>> EditChecklist(EditChecklistRequest model)
        {
            Checklist checklist = await _context.Checklists.FirstOrDefaultAsync((checklist) => checklist.Id.Equals(model.Id));

            if (checklist == null)
            {
                return new CardResponse<ChecklistDTO>() { Message = "Checklist was not found." };
            }

            checklist.Title = model.Title;

            await _context.SaveChangesAsync();

            ChecklistDTO response = DTOConverter.ChecklistToDTO(checklist);

            return new CardResponse<ChecklistDTO>() { Data = response };
        }

        public async Task<CardResponse<ChecklistItemDTO>> EditChecklistItem(EditChecklistItemRequest model)
        {
            ChecklistItem item = await _context.ChecklistItems.FirstOrDefaultAsync((checklist) => checklist.Id.Equals(model.Id));

            if (item == null)
            {
                return new CardResponse<ChecklistItemDTO>() { Message = "Item was not found." };
            }

            item.Content = model.Content;

            await _context.SaveChangesAsync();

            ChecklistItemDTO response = DTOConverter.ChecklistItemToDTO(item);

            return new CardResponse<ChecklistItemDTO>() { Data = response };
        }
        public async Task<CardResponse<int>> CompleteChecklistItem(CompleteChecklistItemRequest model)
        {
            ChecklistItem item = await _context.ChecklistItems.Include(item => item.Checklist).FirstOrDefaultAsync((checklist) => checklist.Id.Equals(model.Id));

            if (item == null)
            {
                return new CardResponse<int>() { Data = -1, Message = "Item was not found." };
            }

            item.Complete = model.Complete;

            List<ChecklistItem> items = await _context.ChecklistItems.Where(i => i.ChecklistId.Equals(item.ChecklistId) && i.Complete).ToListAsync();

            await _context.SaveChangesAsync();

            return new CardResponse<int>() { Data = items.Count };
        }

        public async Task<CardResponse<bool>> ReorderChecklists(ReorderChecklistRequest model)
        {
            List<Checklist> checklists = await _context.Checklists.Where(checklist => checklist.CardId.Equals(model.CardId)).ToListAsync();

            foreach (Checklist checklist in checklists)
            {
                int newOrder = model.Checklists.Find((c) => c.Id == checklist.Id).Order;

                if (newOrder < 0 || String.IsNullOrEmpty(newOrder.ToString()))
                {
                    return new CardResponse<bool> { Data = false, Message = "All checklists require a correct order." };
                }

                checklist.Order = newOrder;
            }

            await _context.SaveChangesAsync();

            return new CardResponse<bool> { Data = true, Message = "Checklist order saved." };
        }

        public async Task<CardResponse<bool>> ReorderChecklistItems(ReorderChecklistItemsRequest model)
        {
            List<ChecklistItem> items = await _context.ChecklistItems.Where(item => item.ChecklistId.Equals(model.ChecklistId)).ToListAsync();

            foreach (ChecklistItem item in items)
            {
                int newOrder = model.ChecklistItems.Find((c) => c.Id == item.Id).Order;

                if (newOrder < 0 || String.IsNullOrEmpty(newOrder.ToString()))
                {
                    return new CardResponse<bool> { Data = false, Message = "All items require a correct order." };
                }

                item.Order = newOrder;
            }

            await _context.SaveChangesAsync();

            return new CardResponse<bool> { Data = true, Message = "Item order saved." };
        }

        public async Task<CardResponse<Guid>> DeleteChecklist(DeleteChecklistRequest model)
        {
            Checklist? checklist = await _context.Checklists.Include(checklist => checklist.Items).FirstOrDefaultAsync(checklist => checklist.Id.Equals(model.Id));

            if (checklist == null)
            {
                return new CardResponse<Guid> { Message = "Checklist was not found." };
            }

            foreach(ChecklistItem item in checklist.Items)
            {
                _context.ChecklistItems.Remove(item);
            }

            _context.Checklists.Remove(checklist);
            await _context.SaveChangesAsync();

            return new CardResponse<Guid> { Data = checklist.CardId, Message ="Checklist deleted." };
        }

        public async Task<DeleteChecklistItemResponse> DeleteChecklistItem(DeleteChecklistItemRequest model)
        {
            ChecklistItem? item = await _context.ChecklistItems.FirstOrDefaultAsync(item => item.Id.Equals(model.Id));

            if (item == null)
            {
                return new DeleteChecklistItemResponse() { CompletedChecklistItems = -1, ChecklistItems = -1, Message = "Item was not found." };
            }

            List<ChecklistItem> items = await _context.ChecklistItems.Where(i => i.ChecklistId.Equals(item.ChecklistId) && !i.Id.Equals(model.Id)).ToListAsync();

            _context.ChecklistItems.Remove(item);
            await _context.SaveChangesAsync();

            return new DeleteChecklistItemResponse() { CompletedChecklistItems = items.FindAll(item => item.Complete).Count, ChecklistItems = items.Count, Message = "Item deleted." };
        }
    }
}
