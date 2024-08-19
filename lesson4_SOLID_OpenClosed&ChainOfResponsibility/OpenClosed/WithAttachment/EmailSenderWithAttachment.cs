using lesson4_SOLID.OpenClosed.Contract;
using System.Net.Mail;

namespace lesson4_SOLID.OpenClosed.WithAttachment
{
    public class EmailSenderWithAttachment : IEmailSender<EmailWithAttachment>
    {
        private IEmailSender<emailContentsBase> _basicSender;
        private string _serverHost;

        public EmailSenderWithAttachment(IEmailSender<emailContentsBase> basicEmailSender, string serverHost)
        {
            _basicSender = basicEmailSender;
            _serverHost = serverHost;
        }

        public void SendEmail(EmailWithAttachment emailContents)
        {
            using (var message = new MailMessage())
            {
                using (var smtpClient = new SmtpClient(_serverHost))
                {
                    if (emailContents.attachments != null && emailContents.attachments.Length > 0)
                    {
                        foreach (var attachment in emailContents.attachments)
                        {
                            var attachmentStream = new System.IO.MemoryStream(attachment.Data);
                            var mailAttachment = new System.Net.Mail.Attachment(attachmentStream, attachment.Name);
                            message.Attachments.Add(mailAttachment);
                        }
                    }

                    SendEmailProcessingChain(emailContents, message, smtpClient);
                }
            }
        }

        public void SendEmailProcessingChain(EmailWithAttachment emailContents, MailMessage message, SmtpClient client)
        {
            _basicSender.SendEmailProcessingChain(emailContents, message, client);
        }
    }
}
