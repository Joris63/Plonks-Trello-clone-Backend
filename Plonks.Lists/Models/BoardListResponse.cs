namespace Plonks.Lists.Models
{
    public class BoardListResponse<T>
    {
        public T? Data { get; set; }

        public string? Message { get; set; }
    }
}
