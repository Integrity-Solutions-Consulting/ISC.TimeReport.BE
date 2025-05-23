﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Shared;

namespace isc.time.report.be.domain.Entity.Customers
{
    public class Customer : BaseEntity
    {
        public int IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string CommercialName { get; set; }
        public string CompanyName { get; set; }
        public string CellPhoneNumber { get; set; }
        public string Email { get; set; }

    }
}