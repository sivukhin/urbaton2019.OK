using CleanCityCore.Model;

namespace CleanCityCore
{
    public interface IUserRepository
    {
        void AddOrUpdateUser(User user);
        User GetUser(long chatId);
    }
}