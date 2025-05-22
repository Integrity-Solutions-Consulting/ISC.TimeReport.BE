using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Projects
{
    public class Project : BaseEntity
    {
        public string ProjectCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
        public int IdCustomer {  get; set; }
        public int IdCompany { get; set; }
        public int IdLeader { get; set; }
    }
}
