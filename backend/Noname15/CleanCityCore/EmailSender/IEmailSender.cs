namespace CleanCityCore.EmailSender
{
    public interface IEmailSender
    {
        void SendEmail(EmailMessage emailMessage);
    }
}