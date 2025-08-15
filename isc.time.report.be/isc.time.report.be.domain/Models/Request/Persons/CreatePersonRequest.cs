﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Persons
{
    public class CreatePersonRequest
    {
        public int? GenderID { get; set; }
        public int? NationalityId { get; set; }
        public int? IdentificationTypeId { get; set; }
        public string? IdentificationNumber { get; set; } // <-- nullable
        public string? PersonType { get; set; }            // <-- nullable
        public string? FirstName { get; set; }             // <-- nullable
        public string? LastName { get; set; }              // <-- nullable
        public DateOnly? BirthDate { get; set; }           // <-- nullable
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

    }
}
