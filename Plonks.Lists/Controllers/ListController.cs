using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plonks.Lists.Models;

namespace Plonks.Lists.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddList([FromBody] AddListRequest model)
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
        [HttpGet("all/{boardId}")]
        public async Task<IActionResult> GetAllListsByBoardId (Guid boardId)
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
        public async Task<IActionResult> EditList([FromBody] EditListRequest model)
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
        public async Task<IActionResult> ArchiveList([FromBody] ArchiveListRequest model)
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
        [HttpDelete("{listId}")]
        public async Task<IActionResult> DeleteList(Guid listId)
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
