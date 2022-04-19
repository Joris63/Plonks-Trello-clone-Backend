using Plonks.Cards.Entities;
using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class ReorderCardsRequest
    {
        [Required]
        public Guid DepartureListId { get; set; }

        [Required]
        public List<CardDTO> DepartureChildren { get; set; } = new List<CardDTO>();


        [Required]
        public Guid DestinationListId { get; set; }

        [Required]
        public List<CardDTO> DestinationChildren { get; set; } = new List<CardDTO>();
    }
}
