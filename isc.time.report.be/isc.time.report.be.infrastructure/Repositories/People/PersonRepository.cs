using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.People;
using isc.time.report.be.domain.Entity.People;
using isc.time.report.be.infrastructure.Database;

namespace isc.time.report.be.infrastructure.Repositories.People
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DBContext dBContext;

        public PersonRepository(DBContext dBContext)
        { 
            this.dBContext = dBContext;
        }

        public async Task<Person> CreatePerson(Person person)
        {
            person.CreatedAt = DateTime.Now;
            person.UpdatedAt = null;
            person.Status = true;
            await dBContext.People.AddAsync(person);
            await dBContext.SaveChangesAsync();
            return person;
        }
    }
}
