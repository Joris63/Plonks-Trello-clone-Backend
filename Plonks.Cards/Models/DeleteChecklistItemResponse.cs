namespace Plonks.Cards.Models
{
    public class DeleteChecklistItemResponse
    {
        public int CompletedChecklistItems { get; set; }

        public int ChecklistItems { get; set; }

        public string Message { get; set; }
    }
}
