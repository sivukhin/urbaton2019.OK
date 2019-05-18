using System;
using System.ComponentModel.DataAnnotations;
using NpgsqlTypes;

namespace CleanCityCore.Sql
{
    public class ReportSql
    {
        [Key] public Guid Id { get; set; }
        public long UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public NpgsqlPoint Location { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Guid ResponsibleId { get; set; }
        public byte[] Payload { get; set; }
    }
}