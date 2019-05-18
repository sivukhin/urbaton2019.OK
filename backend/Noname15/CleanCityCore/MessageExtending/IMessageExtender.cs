using CleanCityCore.Model;

namespace CleanCityCore.MessageExtending
{
    public interface IMessageExtender
    {
        string ExtendSubject(Report report);
        string ExtendReportText(Responsible responsible, User user, Report report);
    }
}