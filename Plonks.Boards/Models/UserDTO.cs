using Plonks.Boards.Entities;

namespace Plonks.Boards.Models
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PicturePath { get; set; }

        public bool isOwner { get; set; } = false;

        public UserDTO()
        {

        }

        public UserDTO(User user, bool isOwner)
        {
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            PicturePath = user.PicturePath;
            this.isOwner = isOwner;
        }
    }
}