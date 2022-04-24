using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plonks.Boards.Entities;
using Plonks.Boards.Models;
using Plonks.Boards.Services;
using Plonks.Shared.Entities;

namespace Plonks.Boards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _service;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ILogger<BoardController> _logger;

        public BoardController(IBoardService service, IPublishEndpoint publishEndpoint, ILogger<BoardController> logger)
        {
            _service = service;
            this.publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddBoard([FromBody] AddBoardRequest model)
        {
            try
            {
                _logger.LogInformation("Adding new board {Board}", model);

                BoardResponse<Guid> response = await _service.AddBoard(model);
                
                if(response.Data == null)
                {
                    return BadRequest(response.Message);
                }

                await publishEndpoint.Publish<QueueMessage<SharedBoard>>(new QueueMessage<SharedBoard>
                {
                    Data = new SharedBoard { Id = response.Data },
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
        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAllUserBoards(Guid userId)
        {
            try
            {
                _logger.LogInformation("Getting all boards by userId {UserId}", userId);

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
        [Route("get")]
        public async Task<IActionResult> GetBoard([FromBody] GetBoardRequest model)
        {
            try
            {
                _logger.LogInformation("Getting board {Board}", model);

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
                _logger.LogInformation("Favoriting board {Board}", model);

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
