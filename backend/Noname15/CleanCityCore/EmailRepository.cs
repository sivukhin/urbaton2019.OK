using System;
using System.Linq;
using System.Text;
using CleanCityCore.EmailSender;
using CleanCityCore.Sql;
using Newtonsoft.Json;

namespace CleanCityCore
{
    public class EmailRepository : IEmailRepository
    {
        public void SetEmailProcessed(Guid emailId)
        {
            using (var context = new CleanCityContext())
            {
                var email = context.Emails.SingleOrDefault(e => e.Id == emailId);
                if (email == null)
                    return;
                context.Emails.Update(new EmailMessageSql
                {
                    Id = emailId,
                    IsSent = true
                });
                context.SaveChanges();
            }
        }

        public EmailMessage ReadEmail(Guid emailId)
        {
            using (var context = new CleanCityContext())
            {
                var email = context.Emails.SingleOrDefault(e => e.Id == emailId);
                if (email == null)
                    return null;
                return DeserializeEmail(email.Data);
            }
        }

        public Guid[] ReadUnsentEmails()
        {
            using (var context = new CleanCityContext())
            {
                var email = context.Emails.Where(e => e.IsSent == false).ToArray();
                return email.Select(x => x.Id).ToArray();
            }
        }

        public Guid AddEmail(EmailMessage email)
        {
            using (var context = new CleanCityContext())
            {
                var id = Guid.NewGuid();
                context.Emails.Add(new EmailMessageSql
                {
                    Id = id,
                    Data = SerializeEmail(email),
                    IsSent = false,
                });
                context.SaveChanges();
                return id;
            }
        }

        private byte[] SerializeEmail(EmailMessage email)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(email));
        }

        private EmailMessage DeserializeEmail(byte[] data)
        {
            return JsonConvert.DeserializeObject<EmailMessage>(Encoding.UTF8.GetString(data));
        }
    }
}