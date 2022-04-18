namespace Plonks.Cards.Models
{
    public class CardResponse<T>
    {
        public T? Data { get; set; }

        public string? Message { get; set; }
    }
}
