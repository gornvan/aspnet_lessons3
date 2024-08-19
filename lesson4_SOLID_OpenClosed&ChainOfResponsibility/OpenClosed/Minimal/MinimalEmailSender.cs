using lesson4_SOLID.OpenClosed.Contract;
using System.Net;
using System.Net.Mail;

namespace lesson4_SOLID.OpenClosed
{
    public class MinimalEmailSender : IEmailSender<emailContentsBase>
    {
        public void SendEmailProcessingChain(emailContentsBase emailContents, MailMessage message, SmtpClient smtpClient)
        {
            message.From = new MailAddress(emailContents.AddressFrom);
            message.To.Add(new MailAddress(emailContents.AddressTo));

            message.Subject = emailContents.Subject;
            message.Body = emailContents.Body;
            message.IsBodyHtml = true; // Assuming the body is HTML, change to false if it's plain text.

            smtpClient.Port = 587; // Typically 587 for SMTP with TLS
            smtpClient.Credentials = new NetworkCredential("username", "password");
            smtpClient.EnableSsl = true; // Set to true if using SSL/TLS

            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                // Handle exceptions, log if necessary
            }
        }

        public void SendEmail(emailContentsBase emailContents)
        {
            using (var message = new MailMessage())
            {
                using (var smtpClient = new SmtpClient("smtp.yourserver.com"))
                {
                    SendEmailProcessingChain(emailContents, message, smtpClient);
                }
            }
        }
    }
}
