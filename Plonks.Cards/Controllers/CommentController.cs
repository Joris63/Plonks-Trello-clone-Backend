using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plonks.Cards.Models;
using Plonks.Cards.Services;
using Plonks.Shared.Entities;

namespace Plonks.Cards.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;
        private readonly IBus _bus;

        public CommentController(ICommentService service, IBus bus)
        {
            _service = service;
            _bus = bus;
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddComment([FromBody] AddCommentRequest model)
        {
            try
            {
                AddCommentResponse response = await _service.AddComment(model);

                if (response.Comment == null)
                {
                    return BadRequest(response.Message);
                }

                await _bus.Publish(new QueueMessage<SharedCard>()
                {
                    Data = new SharedCard()
                    {
                        Id = response.Comment.Id,
                        CommentAmount = response.CommentCount
                    },
                    Type = QueueMessageType.Update
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
        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> EditComment([FromBody] EditCommentRequest model)
        {
            try
            {
                CardResponse<CommentDTO> response = await _service.EditComment(model);

                if (response.Data == null)
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

        [Authorize]
        [HttpPost]
        [Route("remove")]
        public async Task<IActionResult> DeleteComment([FromBody] DeleteCommentRequest model)
        {
            try
            {
                DeleteCommentResponse response = await _service.DeleteComment(model);

                if (response.CardId == Guid.Empty)
                {
                    return BadRequest(response.Message);
                }

                await _bus.Publish(new QueueMessage<SharedCard>()
                {
                    Data = new SharedCard()
                    {
                        Id = response.CardId,
                        CommentAmount = response.CommentCount
                    },
                    Type = QueueMessageType.Update
                });

                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
