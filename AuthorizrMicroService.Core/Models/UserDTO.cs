namespace UserMicroService.Core.Models
{
    public class UserDTO(User user)
    {
        public int Id { get; set; } = user.IdUser;
        public string UserName { get; set; } = user.UserName;
        public string? UserImagePath { get; set; } = user.UserImagePath;
        public DateTime DateOfRegister { get; set; } = user.DateOfRegister;
        public DateTime LastLoginDate { get; set; } = user.LastLoginDate;
    }
}
