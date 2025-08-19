﻿using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Models.Request.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Request.Leaders
{
    public class CreateLeaderWithPersonOBJRequest
    {
        public CreatePersonRequest Person { get; set; }
        public int ProjectID { get; set; }
        public bool LeadershipType { get; set; } = true;
        public DateOnly StartDate { get; set; } 
        public DateOnly? EndDate { get; set; }
        public string? Responsibilities { get; set; }
    }
}
