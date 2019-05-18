using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CleanCityCore.Sql
{
    public class ResponsibleSql
    {
        [Key] public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}