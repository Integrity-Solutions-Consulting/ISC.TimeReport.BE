using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.Persons;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace isc.time.report.be.infrastructure.Repositories.Persons
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DBContext dbContext;

        public PersonRepository(DBContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public async Task<Person> CreatePerson(Person person)
        {
            person.CreatedAt = DateTime.UtcNow;
            person.UpdatedAt = null;
            person.Status = true;
            await dbContext.Person.AddAsync(person);
            await dbContext.SaveChangesAsync();
            return person;
        }

        public async Task<List<Person>> GetPersons()
        {
            return await dbContext.Person
                .Where(p => p.Status)
                .ToListAsync();
        }
    }
}
