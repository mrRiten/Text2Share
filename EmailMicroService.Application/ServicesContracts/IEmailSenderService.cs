namespace EmailMicroService.Application.Services
{
    public interface IEmailSenderService
    {
        public Task SendEmailAsync(string address, string title, string message);
    }
}
