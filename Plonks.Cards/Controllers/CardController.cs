using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plonks.Cards.Entities;
using Plonks.Cards.Models;
using Plonks.Cards.Services;
using Plonks.Shared.Entities;

namespace Plonks.Cards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _service;
        private readonly IPublishEndpoint publishEndpoint;

        public CardController(ICardService service, IPublishEndpoint publishEndpoint)
        {
            _service = service;
            this.publishEndpoint = publishEndpoint;
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddCard([FromBody] AddCardRequest model)
        {
            try
            {
                CardResponse<Card> response = await _service.AddCard(model);

                if (response.Data == null)
                {
                    return BadRequest(response.Message);
                }

                await publishEndpoint.Publish<SharedCard>(new SharedCard { 
                    Id = response.Data.Id, 
                    Title = response.Data.Title, 
                    ListId = response.Data.ListId, 
                    Order = response.Data.Order, 
                    CreatedAt = response.Data.CreatedAt,
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpGet("{cardId}")]
        public async Task<IActionResult> GetCard(Guid cardId)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> EditCard([FromBody] EditCardRequest model)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }


        [Authorize]
        [HttpPost]
        [Route("reorder")]
        public async Task<IActionResult> ReorderCards([FromBody] ReorderCardsRequest model)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }


        [Authorize]
        [HttpPost]
        [Route("archive")]
        public async Task<IActionResult> ArchiveList([FromBody] ArchiveCardRequest model)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
