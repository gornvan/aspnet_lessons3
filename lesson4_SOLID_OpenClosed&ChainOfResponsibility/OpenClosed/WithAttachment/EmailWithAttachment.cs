using lesson4_SOLID.OpenClosed.Contract;

namespace lesson4_SOLID.OpenClosed.WithAttachment
{
    public class EmailWithAttachment : EmailContentsBase
    {
        public required Attachment[] attachments;
    }
}