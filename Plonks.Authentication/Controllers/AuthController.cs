using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plonks.Auth.Models;
using Plonks.Auth.Services;

namespace Plonks.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                AuthenticateResponse response = await _service.Register(model);

                if(response.AccessToken == null)
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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                AuthenticateResponse response = await _service.Login(model);

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

                if(!result)
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
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                Secure = true,
                SameSite = SameSiteMode.None,
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
