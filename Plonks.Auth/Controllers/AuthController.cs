using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plonks.Auth.Models;
using Plonks.Auth.Services;
using Plonks.Shared.Entities;

namespace Plonks.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IBus _bus;

        public AuthController(IAuthService service, IBus bus)
        {
            _service = service;
            _bus = bus;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                AuthenticateResponse response = await _service.Register(model);

                if (response.AccessToken == null)
                {
                    return BadRequest(response.Message);
                }

                await _bus.Publish(new QueueMessage<SharedUser>
                {
                    Data = new SharedUser { Id = response.Id, Username = response.Username, Email = response.Email, PicturePath = response.PicturePath },
                    Type = QueueMessageType.Insert
                });

                setTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                AuthenticateResponse response = await _service.Login(model);

                if (response.AccessToken == null)
                {
                    return Unauthorized();
                }

                setTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("social-login")]
        public async Task<IActionResult> SocialLogin([FromBody] SocialLoginRequest model)
        {
            try
            {
                AuthenticateResponse response = await _service.SocialLogin(model);

                if (response.AccessToken == null)
                {
                    return BadRequest(response.Message);
                }

                if (response.Message == "registered")
                {
                    await _bus.Publish(new QueueMessage<SharedUser>
                    {
                        Data = new SharedUser { Id = response.Id, Username = response.Username, Email = response.Email, PicturePath = response.PicturePath },
                        Type = QueueMessageType.Insert
                    });
                }

                setTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                string refreshToken = Request.Cookies["refreshToken"];

                if (refreshToken == null)
                {
                    return BadRequest("Invalid token.");
                }

                RefreshTokenResponse response = await _service.RefreshToken(refreshToken);

                if (response.AccessToken == null)
                {
                    return BadRequest(response.Message);
                }

                setTokenCookie(response.RefreshToken);

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
        [Route("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            try
            {
                bool result = await _service.RevokeToken(model);

                if (!result)
                {
                    return BadRequest("Something wen wrong.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7),
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
