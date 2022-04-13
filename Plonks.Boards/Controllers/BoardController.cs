using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plonks.Boards.Entities;
using Plonks.Boards.Models;
using Plonks.Boards.Services;

namespace Plonks.Boards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _service;
        private readonly IPublishEndpoint publishEndpoint;

        public BoardController(IBoardService service, IPublishEndpoint publishEndpoint)
        {
            _service = service;
            this.publishEndpoint = publishEndpoint;
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddBoard([FromBody] AddBoardRequest model)
        {
            try
            {
                BoardResponse<Guid> response = await _service.AddBoard(model);
                
                if(response.Data == null)
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
        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAllUserBoards(Guid userId)
        {
            try
            {
                BoardResponse<List<BoardDTO>> response = await _service.GetAllUserBoards(userId);

                if(response.Data == null)
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
        public async Task<IActionResult> GetBoard([FromBody] GetBoardRequest model)
        {
            try
            {
                BoardResponse<BoardDTO> response = await _service.GetBoard(model);

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
        [Route("favorite")]
        public async Task<IActionResult> FavoriteBoard([FromBody] FavoriteBoardRequest model)
        {
            try
            {
                BoardResponse<Guid> response = await _service.FavoriteBoard(model);

                if (response.Data == null)
                {
                    return BadRequest(response.Message);
                }

                return Ok(response.Data);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
