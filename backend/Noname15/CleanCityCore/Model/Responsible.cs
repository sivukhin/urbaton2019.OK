using System;

namespace CleanCityCore.Model
{
    public class Responsible
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ResponseRegion { get; set; }
        public bool IsActive { get; set; }
    }
}
