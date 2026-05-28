using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace FinTrack.Service.IService
{
    public interface IEmailService : IEmailSender
    {
        Task SendEmailWithAttachmentAsync(string email, string subject, string htmlMessage, byte[] attachments, string fileName);
    }
}
