using CleanCityCore.EmailSender;
using CleanCityCore.Model;

namespace CleanCityCore
{
    public class InitialReport
    {
        public GeoLocation Location { get; set; }
        public Attachment[] Attachments { get; set; }
        public string Subject { get; set; }
        public string ReportText { get; set; }
    }
}