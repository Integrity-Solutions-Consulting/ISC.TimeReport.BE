using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.People
{
    public class CreatePersonResponse
    {
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string Names { get; set; }
        public string Surnames { get; set; }
        public string Gender { get; set; }
        public string CellPhoneNumber { get; set; }
        public string Position { get; set; }
        public string PersonalEmail { get; set; }
        public string CorporateEmail { get; set; }
        public string HomeAddress { get; set; }
    }
}
