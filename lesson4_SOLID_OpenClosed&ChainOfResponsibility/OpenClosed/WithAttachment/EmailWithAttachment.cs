using lesson4_SOLID.OpenClosed.Contract;

namespace lesson4_SOLID.OpenClosed.WithAttachment
{
    public class EmailWithAttachment : emailContentsBase
    {
        public required Attachment[] attachments;
    }
}