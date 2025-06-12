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
    public class Person : BaseEntity
    {
        public int? GenderID { get; set; }
        public Gender Gender { get; set; }
        public int? NationalityId { get; set; }
        public Nationality Nationality { get; set; }
        public int? IdentificationTypeId { get; set; }
        public IdentificationType IdentificationType { get; set; }
        public string IdentificationNumber { get; set; } = null!;
        public string PersonType { get; set; } = "NATURAL";
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
