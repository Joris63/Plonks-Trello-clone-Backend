namespace Plonks.Boards.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PicturePath { get; set; }

        public bool SocialAccount { get; set; }
    }
}
