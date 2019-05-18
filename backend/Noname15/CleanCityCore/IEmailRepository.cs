using System;
using CleanCityCore.EmailSender;

namespace CleanCityCore
{
    public interface IEmailRepository
    {
        void SetEmailProcessed(Guid emailId);
        EmailMessage ReadEmail(Guid emailId);
        Guid[] ReadUnsentEmails();
        Guid AddEmail(EmailMessage email);
    }
}