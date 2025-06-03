using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Leaders
{
    [Table("Leaders")]
    public class Leader : BaseEntity
    {
        public string LeaderType { get; set; }
        public string ProjectCode { get; set; }
        public string CustomerCode { get; set; }
        public int IdPerson { get; set; }
        public Person Person { get; set; }
    }
}
