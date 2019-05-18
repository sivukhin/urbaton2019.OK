using CleanCityCore.Model;

namespace CleanCityCore
{
    public interface IResponsibleFounder
    {
        Responsible GetResponsible(GeoLocation location);
    }
}