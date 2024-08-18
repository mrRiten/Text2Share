namespace EmailMicroService.Core.Models
{
    public class SMTP
    {
        public required string Server { get; set; }
        public required int Port { get; set; }
        public required string Address { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
    }
}
