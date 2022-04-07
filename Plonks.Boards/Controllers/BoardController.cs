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

        public BoardController(IBoardService service)
        {
            _service = service;
        }

        //[Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddBoard([FromBody] AddBoardRequest model)
        {
            try
            {
                BoardResponse<Board> response = await _service.AddBoard(model);
                
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

        //[Authorize]
        [HttpGet("{userId}")]
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
    }
}
