using System;

namespace BackendApi.Controllers
{
    public class AddDoublerQuery
    {
        public Guid ResponsibleId { get; set; }
        public ResponsibleDoubler Doubler { get; set; }
    }
}