using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plonks.Lists.Models;
using Plonks.Lists.Services;
using Plonks.Shared.Entities;

namespace Plonks.Lists.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly IListService _service;
        private readonly IPublishEndpoint publishEndpoint;

        public ListController(IListService service, IPublishEndpoint publishEndpoint)
        {
            _service = service;
            this.publishEndpoint = publishEndpoint;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddList([FromBody] AddListRequest model)
        {
            try
            {
                BoardListResponse<Guid> response = await _service.AddList(model);

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
        [HttpGet("all/{boardId}")]
        public async Task<IActionResult> GetAllListsByBoardId(Guid boardId)
        {
            try
            {
                BoardListResponse<List<BoardListDTO>> response = await _service.GetAllListsByBoardId(boardId);

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
        public async Task<IActionResult> EditList([FromBody] EditListRequest model)
        {
            try
            {
                BoardListResponse<Guid> response = await _service.EditList(model);

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
        [Route("archive")]
        public async Task<IActionResult> ArchiveList([FromBody] ArchiveListRequest model)
        {
            try
            {
                BoardListResponse<Guid> response = await _service.ArchiveList(model);

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
    }
}
