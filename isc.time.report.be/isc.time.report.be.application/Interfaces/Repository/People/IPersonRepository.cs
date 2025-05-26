using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.People;

namespace isc.time.report.be.application.Interfaces.Repository.People
{
    public interface IPersonRepository
    {
        public Task<Person> CreatePerson(Person person);
    }
}
