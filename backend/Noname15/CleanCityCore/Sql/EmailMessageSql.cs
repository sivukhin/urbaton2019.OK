using System;
using System.ComponentModel.DataAnnotations;

namespace CleanCityCore.Sql
{
    public class EmailMessageSql
    {
        [Key] 
        public Guid Id { get; set; }
        public bool IsSent { get; set; }
        public byte[] Data { get; set; }
    }
}