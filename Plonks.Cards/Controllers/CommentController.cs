using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plonks.Cards.Models;

namespace Plonks.Cards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddComment([FromBody] AddCommentRequest model)
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
        public async Task<IActionResult> EditComment([FromBody] EditCommentRequest model)
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
        [Route("remove")]
        public async Task<IActionResult> DeleteComment([FromBody] DeleteCommentRequest model)
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
