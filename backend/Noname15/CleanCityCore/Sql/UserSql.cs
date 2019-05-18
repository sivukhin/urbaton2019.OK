using System.ComponentModel.DataAnnotations;

namespace CleanCityCore.Sql
{
    public class UserSql
    {
        [Key] public long ChatId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}