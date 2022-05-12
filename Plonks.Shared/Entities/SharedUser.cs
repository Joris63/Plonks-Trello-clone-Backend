namespace Plonks.Shared.Entities
{
    public class SharedUser
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PicturePath { get; set; }
    }
}
