using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using FinTrack.Models;
using Microsoft.Extensions.Options;
using System.Configuration;


namespace FinTrack.Dummy
{
    public class EmailService : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("user", email));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = htmlMessage
            };

            using (var client = new SmtpClient())
            {
                //Disables SSL certificate veriication
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            
        }
    }
}
