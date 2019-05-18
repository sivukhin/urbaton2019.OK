using System.Linq;
using CleanCityCore.Model;
using CleanCityCore.Sql;

namespace CleanCityCore
{
    public class UserRepository : IUserRepository
    {
        public void AddOrUpdateUser(User user)
        {
            using (var context = new CleanCityContext())
            {
                var sqlUser = context.Users.SingleOrDefault(x => x.UserId == user.UserId);
                if (sqlUser == null)
                {
                    context.Users.Add(new UserSql
                    {
                        Email = user.Email,
                        UserId = user.UserId,
                        Address = user.Address,
                        Username = user.Username,
                    });
                    context.SaveChanges();
                }
                else
                {
                    sqlUser.Email = user.Email;
                    sqlUser.Username = user.Username;
                    sqlUser.Address = user.Address;
                    context.Update(sqlUser);
                    context.SaveChanges();
                }
            }
        }

        public User GetUser(long chatId)
        {
            using (var context = new CleanCityContext())
            {
                var sqlUser = context.Users.SingleOrDefault(x => x.UserId == chatId);
                if (sqlUser == null)
                    return null;
                return new User
                {
                    Email = sqlUser.Email,
                    Address = sqlUser.Address,
                    Username = sqlUser.Username,
                    UserId = sqlUser.UserId,
                };
            }
        }
    }
}