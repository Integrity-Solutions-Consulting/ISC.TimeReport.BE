using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Persons;

namespace isc.time.report.be.application.Interfaces.Repository.Persons
{
    public interface IPersonRepository
    {
        public Task<List<Person>> GetPersons();

        public Task<Person> CreatePerson(Person person);
    }
}
