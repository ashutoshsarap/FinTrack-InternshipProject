using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;


namespace FinTrack.Dummy
{
    public class DummyEmailService : IEmailSender
    {

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("FinTrack", "ashutoshsarapgdsc@gmail.com"));
            message.To.Add(new MailboxAddress("user", email));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = """
                Hi, how are yu?
                """
            };

            using (var client = new SmtpClient())
            {
                //Disables SSL certificate veriication
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("ashutoshsarapgdsc@gmail.com", "zreh pzzx poef oimx");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
