using System;

namespace CleanCityCore.Model
{
    public class ReportPreview
    { 
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Subject { get; set; }
        public string ReportText { get; set; }
    }
}
