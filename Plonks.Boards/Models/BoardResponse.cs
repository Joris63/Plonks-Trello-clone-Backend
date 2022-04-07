using Plonks.Boards.Entities;

namespace Plonks.Boards.Models
{
    public class BoardResponse<T>
    {
        public T? Data { get; set; }

        public string? Message { get; set; }
    }
}
