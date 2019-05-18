namespace CleanCityCore.EmailSender
{
    public class EmailMessage
    {
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Attachment[] Attachments { get; set; }
    }
}