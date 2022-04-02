using Plonks.Auth.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace Plonks.Auth.Models
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PicturePath { get; set; }

        public string? AccessToken { get; set; }

        //[JsonIgnore]
        public string? RefreshToken { get; set; }

        public string Message { get; set; } = "";


        public AuthenticateResponse(string message)
        {
            Message = message;
        }

        public AuthenticateResponse(User user, string accessToken, string refreshToken)
        {
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            PicturePath = user.PicturePath;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public AuthenticateResponse(User user, string accessToken, string refreshToken, string message)
        {
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            PicturePath = user.PicturePath;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Message = message;
        }
    }
}
