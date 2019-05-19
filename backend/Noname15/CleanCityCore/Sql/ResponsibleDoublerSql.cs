using System;
using System.ComponentModel.DataAnnotations;

namespace CleanCityCore.Sql
{
    public class ResponsibleDoublerSql
    {
        [Key] public Guid Id { get; set; }
        public Guid OriginalId { get; set; }
        public ResponsibleSql OriginalResponsible { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}