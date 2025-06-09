using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Genders;
using isc.time.report.be.domain.Entity.Nationalities;
using isc.time.report.be.domain.Entity.IdentificationTypes;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Persons
{
    [Table("Persons")]
    public class Person : BaseEntity
    {
        public int GenderId { get; set; }
        public int NationalityId { get; set; }
        public int IdentificationTypeId { get; set; }
        public string IdentificationNumber { get; set; }
        public string PersonType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Gender? Gender { get; set; }
        public Nationality? Nationality { get; set; }
        public IdentificationType? IdentificationType { get; set; }
    }
}
