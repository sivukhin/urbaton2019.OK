using System;
using CleanCityCore.Model;

namespace CleanCityCore
{
    public interface IReportRepository
    {
        Guid AddReport(Report report);
        Guid[] ReadReports();
        Guid[] ReadReportsOfResponsible(Guid responsibleId);

        Report ReadReport(Guid reportId);
    }
}