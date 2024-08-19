using System.Net.Mail;

namespace lesson4_SOLID.OpenClosed.Contract
{
    public interface IEmailSender<EmailType> where EmailType : emailContentsBase
    {
        void sendEmail(EmailType emailContents);
        void SendEmailProcessingChain(EmailType emailContents, MailMessage message, SmtpClient client);
    }
}
