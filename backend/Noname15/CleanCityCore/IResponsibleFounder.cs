using CleanCityCore.Model;

namespace CleanCityCore
{
    public interface IResponsibleFounder
    {
        Responsible[] GetAllResponsibles();
        Responsible GetResponsible(GeoLocation location);
    }
}