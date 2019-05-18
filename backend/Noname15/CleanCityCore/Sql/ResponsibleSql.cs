using System;
using System.ComponentModel.DataAnnotations;

namespace CleanCityCore.Sql
{
    public class ResponsibleSql
    {
        [Key] public Guid Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}