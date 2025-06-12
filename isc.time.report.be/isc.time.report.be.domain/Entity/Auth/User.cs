using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Modules;
using isc.time.report.be.domain.Entity.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace isc.time.report.be.domain.Entity.Auth
{
    public class User : BaseEntity
    {
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime? LastLogin { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool? MustChangePassword { get; set; } = true;
        public List<UserRole> UserRole { get; set; }
    }
}
