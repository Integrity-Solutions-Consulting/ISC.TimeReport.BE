using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Models.Response.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Leaders
{
    public class AssignPersonToProjectRequest
    {
        public int PersonID { get; set; }
        public List<PersonProjectMiddleRequest> PersonProjectMiddle { get; set; }
    }
    public class PersonProjectMiddleRequest
    {
        public int Id { get; set; }
        public int ProjectID { get; set; }
        public bool LeadershipType { get; set; } = true;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Responsibilities { get; set; }
        public bool Status { get; set; }
    }
}
