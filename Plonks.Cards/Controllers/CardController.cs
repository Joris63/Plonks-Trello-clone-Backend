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
                CardResponse<CardDTO> response = await _service.AddCard(model);

                if (response.Data == null)
                {
                    return BadRequest(response.Message);
                }

                await publishEndpoint.Publish<QueueMessage<SharedCard>>(new QueueMessage<SharedCard>()
                {
                    Data = new SharedCard()
                    {
                        Id = response.Data.Id,
                        Title = response.Data.Title,
                        ListId = response.Data.ListId,
                        Order = response.Data.Order,
                        CreatedAt = response.Data.CreatedAt,
                    },
                    Type = QueueMessageType.Insert
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
                CardResponse<CardDTO> response = await _service.GetCard(cardId);

                if (response.Data == null)
                {
                    return BadRequest(response.Message);
                }

                return Ok(response.Data);
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
                CardResponse<CardDTO> response = await _service.EditCard(model);

                if (response.Data == null)
                {
                    return BadRequest(response.Message);
                }

                await publishEndpoint.Publish<QueueMessage<SharedCard>>(new QueueMessage<SharedCard>()
                {
                    Data = new SharedCard()
                    {
                        Id = model.Id,
                        Title = response.Data.Title,
                        HasDescription = response.Data.Description != null || response.Data.Title != String.Empty,
                    },
                    Type = QueueMessageType.Update
                });

                return Ok(response.Data);
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
                CardResponse<bool> response = await _service.ReorderCards(model);

                if (!response.Data)
                {
                    return BadRequest(response.Message);
                }

                return Ok(response.Message);
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
                CardResponse<Guid> response = await _service.ArchiveCard(model);

                if (response.Data == Guid.Empty)
                {
                    return BadRequest(response.Message);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
