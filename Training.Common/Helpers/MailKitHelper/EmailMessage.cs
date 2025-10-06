using MimeKit;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.Helpers.MailKitHelper
{
    [ExcludeFromCodeCoverage]
    public class EmailMessage
    {
        public MailboxAddress Sender { get; set; }
        public MailboxAddress Receiver { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
