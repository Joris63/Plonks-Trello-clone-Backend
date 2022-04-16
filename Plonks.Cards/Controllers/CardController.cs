using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plonks.Cards.Models;

namespace Plonks.Cards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddCard([FromBody] AddCardRequest model)
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
