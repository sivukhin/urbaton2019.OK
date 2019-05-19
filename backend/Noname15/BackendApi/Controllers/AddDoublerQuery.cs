using System;
using CleanCityCore.Model;

namespace BackendApi.Controllers
{
    public class AddDoublerQuery
    {
        public Guid ResponsibleId { get; set; }
        public Responsible Doubler { get; set; }
    }
}