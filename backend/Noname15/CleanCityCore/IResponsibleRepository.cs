using System;
using CleanCityCore.Model;

namespace CleanCityCore
{
    public interface IResponsibleRepository
    {
        Responsible ReadResponsible(Guid responsibleId);
        Guid[] ReadResponsibles();
        Guid AddResponsible(Responsible responsible);
        Responsible[] GetDoublers(Guid responsibleId);
    }
}