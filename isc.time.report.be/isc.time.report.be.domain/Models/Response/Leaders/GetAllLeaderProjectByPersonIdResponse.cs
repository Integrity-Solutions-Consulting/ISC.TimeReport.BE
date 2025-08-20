using isc.time.report.be.domain.Models.Response.Persons;
using isc.time.report.be.domain.Models.Response.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Leaders
{
    public class GetAllLeaderProjectByPersonIdResponse
    {
        public GetPersonResponse? Person { get; set; }
        public List<LeaderData> LeaderMiddle { get; set; }
    }
    public class LeaderData
    {
        public int Id { get; set; }
        public string Responsibility { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool LeadershipType { get; set; }
        public bool Status { get; set; }
        public GetAllProjectsResponse? Projectos { get; set; }
    }

}
