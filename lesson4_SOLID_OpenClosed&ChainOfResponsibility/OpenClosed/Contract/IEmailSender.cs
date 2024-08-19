using System.Net.Mail;
using lesson4_SOLID.OpenClosed.Base;

namespace lesson4_SOLID.OpenClosed.Contract
{
    public interface IEmailSender<EmailType> where EmailType : EmailContentsBase
    {
        void SendEmail(EmailType emailContents);
        void SendEmailProcessingChain(EmailType emailContents, MailMessage message, SmtpClient client);
    }
}
