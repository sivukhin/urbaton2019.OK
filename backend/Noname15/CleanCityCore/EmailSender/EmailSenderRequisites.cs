namespace CleanCityCore.EmailSender
{
    public class EmailSenderRequisites
    {
        public string ServerEmail { get; set; }
        public string ServerLogin { get; set; }
        public string ServerPassword { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
    }
}