namespace Plonks.Lists.Models
{
    public class BoardListDTO
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public Guid BoardId { get; set; }

        public List<CardDTO> Cards { get; set; } = new List<CardDTO>();
    }
}
