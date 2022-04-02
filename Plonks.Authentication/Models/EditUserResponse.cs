using Plonks.Auth.Entities;

namespace Plonks.Auth.Models
{
    public class EditUserResponse
    {
        public Guid Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PicturePath { get; set; }

        public bool SocialLogin { get; set; }

        public string? AccessToken { get; set; }

        public string Message { get; set; } = "";


        public EditUserResponse(string message)
        {
            Message = message;
        }

        public EditUserResponse(User user, string accessToken)
        {
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            PicturePath = user.PicturePath;
            SocialLogin = user.SocialAccount;
            AccessToken = accessToken;
        }
        public EditUserResponse(User user, string accessToken, string message)
        {
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            PicturePath = user.PicturePath;
            SocialLogin = user.SocialAccount;
            AccessToken = accessToken;
            Message = message;
        }
    }
}
