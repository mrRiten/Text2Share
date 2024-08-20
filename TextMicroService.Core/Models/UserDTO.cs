namespace UserMicroService.Core.Models
{
    public class UserDTO()
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public string? UserImagePath { get; set; }
        public DateTime DateOfRegister { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
