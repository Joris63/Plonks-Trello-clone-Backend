using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plonks.Cards.Models;
using Plonks.Cards.Services;
using Plonks.Shared.Entities;

namespace Plonks.Cards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistController : ControllerBase
    {
        private readonly IChecklistService _service;
        private readonly IPublishEndpoint publishEndpoint;

        public ChecklistController(IChecklistService service, IPublishEndpoint publishEndpoint)
        {
            _service = service;
            this.publishEndpoint = publishEndpoint;
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddChecklist([FromBody] AddChecklistRequest model)
        {
            try
            {
                CardResponse<ChecklistDTO> response = await _service.AddChecklist(model);

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
        [Route("add-item")]
        public async Task<IActionResult> AddChecklistItem([FromBody] AddChecklistItemRequest model)
        {
            try
            {
                AddChecklistItemResponse response = await _service.AddChecklistItem(model);

                if (response.ChecklistItem == null)
                {
                    return BadRequest(response.Message);
                }

                await publishEndpoint.Publish<QueueMessage<SharedCard>>(new QueueMessage<SharedCard>()
                {
                    Data = new SharedCard()
                    {
                        Id = response.ChecklistItem.Id,
                        ChecklistItems = response.ChecklistItemsCount,
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
        public async Task<IActionResult> EditChecklist([FromBody] EditChecklistRequest model)
        {
            try
            {
                CardResponse<ChecklistDTO> response = await _service.EditChecklist(model);

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
        [Route("edit-item")]
        public async Task<IActionResult> EditChecklistItem([FromBody] EditChecklistItemRequest model)
        {
            try
            {
                CardResponse<ChecklistItemDTO> response = await _service.EditChecklistItem(model);

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
        [Route("complete-item")]
        public async Task<IActionResult> CompleteChecklistItem([FromBody] CompleteChecklistItemRequest model)
        {
            try
            {
                CardResponse<int> response = await _service.CompleteChecklistItem(model);

                if (response.Data == -1)
                {
                    return BadRequest(response.Message);
                }

                await publishEndpoint.Publish<QueueMessage<SharedCard>>(new QueueMessage<SharedCard>()
                {
                    Data = new SharedCard()
                    {
                        Id = model.Id,
                        CompletedChecklistItems = response.Data,
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
        [Route("reorder-items")]
        public async Task<IActionResult> ReorderChecklistItems([FromBody] ReorderChecklistItemsRequest model)
        {
            try
            {
                CardResponse<bool> response = await _service.ReorderChecklistItems(model);

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
        [Route("remove")]
        public async Task<IActionResult> DeleteChecklist([FromBody] DeleteChecklistRequest model)
        {
            try
            {
                CardResponse<Guid> response = await _service.DeleteChecklist(model);

                if (response.Data == Guid.Empty)
                {
                    return BadRequest(response.Message);
                }

                await publishEndpoint.Publish<QueueMessage<SharedCard>>(new QueueMessage<SharedCard>()
                {
                    Data = new SharedCard()
                    {
                        Id = response.Data,
                        ChecklistItems = 0,
                        CompletedChecklistItems = 0,
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

        [Authorize]
        [HttpPost]
        [Route("remove-item")]
        public async Task<IActionResult> DeleteChecklistItem([FromBody] DeleteChecklistItemRequest model)
        {
            try
            {
                DeleteChecklistItemResponse response = await _service.DeleteChecklistItem(model);

                if (response.CompletedChecklistItems != -1 && response.ChecklistItems == -1)
                {
                    return BadRequest(response.Message);
                }

                await publishEndpoint.Publish<QueueMessage<SharedCard>>(new QueueMessage<SharedCard>()
                {
                    Data = new SharedCard()
                    {
                        Id = model.Id,
                        ChecklistItems = response.ChecklistItems,
                        CompletedChecklistItems = response.CompletedChecklistItems,
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
