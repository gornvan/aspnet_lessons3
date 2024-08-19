namespace lesson4_SOLID.NotOpenClosed
{
    public interface IEmailSender
    {
        void SendEmail(string email, string addressFrom, string addressTo, string body);

        void SendEmail(string email, string addressFrom, string addressTo, string body, byte[] attachment, string attachmentName);

        void SendEmail(string email, string addressFrom, string addressTo, string[] cc, string body, byte[] attachment, string attachmentName);
    }
}
