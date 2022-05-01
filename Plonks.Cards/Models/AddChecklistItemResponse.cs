namespace Plonks.Cards.Models
{
    public class AddChecklistItemResponse
    {
        public ChecklistItemDTO? ChecklistItem { get; set; }

        public int ChecklistItemsCount { get; set; }

        public string Message { get; set; }
    }
}
