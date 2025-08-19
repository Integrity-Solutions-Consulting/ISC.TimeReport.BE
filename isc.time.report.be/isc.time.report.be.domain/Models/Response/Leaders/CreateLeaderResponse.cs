using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Models.Response.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Leaders
{
    public class CreateLeaderResponse
    {
        public int Id { get; set; }
        public CreatePersonResponse Person { get; set; }
        public int ProjectID { get; set; }
        public bool LeadershipType { get; set; } = true;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Responsibilities { get; set; }
        public bool Status { get; set; }
    }
}
