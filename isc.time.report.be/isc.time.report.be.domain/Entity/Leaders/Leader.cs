using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Leaders
{
    public class Leader : BaseEntity
    {
        public int PersonID { get; set; }
        public Person Person { get; set; }
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public bool LeadershipType { get; set; } = true;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Responsibilities { get; set; }
    }
}
