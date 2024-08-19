using System.Net;
using System.Net.Mail;


namespace lesson4_SOLID.NotOpenClosed
{
    public class EmailSender : IEmailSender
    {
        private const string EmailFrom = "me@example.com"; // not configurable -- not open to exending

        public void SendEmail(string email, string addressFrom, string addressTo, string body)
        {
            SendEmail(email, addressFrom, addressTo, new string[] { }, body, null, null);
        }

        public void SendEmail(string email, string addressFrom, string addressTo, string body, byte[] attachment, string attachmentName)
        {
            SendEmail(email, addressFrom, addressTo, new string[] { }, body, attachment, attachmentName);
        }

        public void SendEmail(string email,
            string? addressFrom,
            string addressTo,
            string[] cc,
            string body,
            byte[] attachment,
            string attachmentName)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(addressFrom ?? EmailFrom);
                message.To.Add(new MailAddress(addressTo));
                if (cc != null)
                {
                    foreach (var ccAddress in cc)
                    {
                        message.CC.Add(new MailAddress(ccAddress));
                    }
                }
                message.Subject = email;
                message.Body = body;
                message.IsBodyHtml = true; // Assuming the body is HTML, change to false if it's plain text.

                if (attachment != null && attachment.Length > 0)
                {
                    var attachmentStream = new System.IO.MemoryStream(attachment);
                    var mailAttachment = new Attachment(attachmentStream, attachmentName);
                    message.Attachments.Add(mailAttachment);
                }

                using (var smtpClient = new SmtpClient("smtp.yourserver.com"))
                {
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
            }
        }
    }

}
