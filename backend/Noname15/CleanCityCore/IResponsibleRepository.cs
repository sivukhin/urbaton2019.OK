using System;

namespace CleanCityCore
{
    public interface IResponsibleRepository
    {
        Responsible ReadResponsible(Guid responsibleId);
        Guid[] ReadResponsibles();
        Guid AddResponsible(Responsible responsible);
    }
}