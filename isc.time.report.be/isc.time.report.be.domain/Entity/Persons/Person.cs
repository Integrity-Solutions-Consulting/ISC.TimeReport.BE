using isc.time.report.be.domain.Entity.Catalogs;
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
        public DateOnly? BirthDate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

    }
}
