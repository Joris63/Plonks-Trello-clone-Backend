using System.Text.Json.Serialization;

namespace Plonks.Auth.Models
{
    public class RefreshTokenResponse
    {
        public string? AccessToken { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        
        public string? Message { get; set; }

        public RefreshTokenResponse(string message)
        {
            Message = message;
        }

        public RefreshTokenResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
