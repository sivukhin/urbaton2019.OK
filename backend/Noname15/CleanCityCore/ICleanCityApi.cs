using System;
using CleanCityCore.Model;

namespace CleanCityCore
{
    public interface ICleanCityApi
    {
        Responsible GetResponsible(Guid responsibleId);
        Responsible[] GetResponsibles(int start, int count);
        ReportPreview[] GetReports(Guid responsibleId, int start, int count);
        ReportPreview[] GetReports(int start, int count);
        Report GetReport(Guid reportId);
        Guid SendReport(InitialReport report);
    }
}