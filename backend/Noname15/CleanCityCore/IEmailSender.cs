namespace CleanCityCore
{
    public interface IEmailSender
    {
        void SendEmail(EmailMessage emailMessage);
    }
}