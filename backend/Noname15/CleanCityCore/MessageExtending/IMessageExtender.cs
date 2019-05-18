namespace CleanCityCore.MessageExtending
{
    public interface IMessageExtender
    {
        string ExtendSubject(string text);
        string ExtendReportText(string responsibleName, string text);
    }
}