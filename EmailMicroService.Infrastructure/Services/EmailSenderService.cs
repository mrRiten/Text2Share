using EmailMicroService.Application.Services;
using EmailMicroService.Core.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailMicroService.Infrastructure.Services
{
    public class EmailSenderService(IOptions<SMTP> sMTP) : IEmailSenderService
    {
        private readonly SMTP _sMTP = sMTP.Value;

        public async Task SendEmailAsync(string address, string title, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_sMTP.Name, _sMTP.Address));
            emailMessage.To.Add(new MailboxAddress("", address));
            emailMessage.Subject = title;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using var client = new SmtpClient();
            await client.ConnectAsync(_sMTP.Server, int.Parse(_sMTP.Port), true);
            await client.AuthenticateAsync(_sMTP.Address, _sMTP.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
