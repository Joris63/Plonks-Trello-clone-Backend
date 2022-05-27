using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plonks.Auth.Models;
using Plonks.Auth.Services;
using Plonks.Shared.Entities;

namespace Plonks.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IBus _bus;

        public UserController(IUserService service, IBus bus)
        {
            _service = service;
            _bus = bus;
        }

        [Authorize]
        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] EditUserRequest model)
        {
            try
            {
                EditUserResponse response = await _service.Edit(model);

                if(response.Id == Guid.Empty)
                {
                    return BadRequest(response.Message);
                }

                await _bus.Publish(new QueueMessage<SharedUser>
                {
                    Data = new SharedUser { Id = response.Id, Username = response.Username, Email = response.Email, PicturePath = response.PicturePath },
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
        [Route("edit-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest model)
        {
            try
            {
                EditUserResponse response = await _service.ChangePassword(model);

                if (response.Id == Guid.Empty)
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
