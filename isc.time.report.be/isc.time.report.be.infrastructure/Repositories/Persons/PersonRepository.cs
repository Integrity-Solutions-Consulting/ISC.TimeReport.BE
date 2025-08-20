﻿using isc.time.report.be.application.Interfaces.Repository.Persons;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Utils.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Persons
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DBContext _dbContext;

        public PersonRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<Person>> GetAllPersonsPaginatedAsync(PaginationParams paginationParams, string? search)
        {
            var query = _dbContext.Persons
                .Include(p => p.Gender)
                .Include(p => p.Nationality)
                .Include(p => p.IdentificationType)
                .AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                string normalizedSearch = search.Trim().ToLower();

                query = query.Where(e =>
                (e.FirstName != null && e.FirstName.ToLower().Contains(normalizedSearch)) ||
                (e.LastName != null && e.LastName.ToLower().Contains(normalizedSearch)) ||
                (e.IdentificationNumber != null && e.IdentificationNumber.ToLower().Contains(normalizedSearch)));

            }

            return await PaginationHelper.CreatePagedResultAsync(query, paginationParams);
        }

        public async Task<Person> GetPersonByIDAsync(int personId)
        {
            if (personId <= 0)
            {
                throw new ClientFaultException("El ID de la persona es inválido.");
            }

            var person = await _dbContext.Persons
                .Include(p => p.Gender)
                .Include(p => p.Nationality)
                .Include(p => p.IdentificationType)
                .FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
            {
                throw new ClientFaultException($"No se encontró la persona con ID {personId}.");
            }
            return person;
        }

        public async Task<Person> CreatePersonAsync(Person person)
        {
            person.CreationDate = DateTime.Now;
            person.Status = true;
            person.CreationUser = "SYSTEM";

            await _dbContext.Persons.AddAsync(person);
            await _dbContext.SaveChangesAsync();

            return person;
        }

        public async Task<Person> UpdatePersonAsync(Person person)
        {
            person.ModificationDate = DateTime.Now;
            person.ModificationUser = "SYSTEM";
            _dbContext.Entry(person).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return person;
        }

        public async Task<Person> InactivatePersonAsync(int personId)
        {
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
                throw new ClientFaultException($"La persona con ID {personId} no existe.");

            person.Status = false;
            person.ModificationDate = DateTime.Now;
            _dbContext.Entry(person).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return person;
        }

        public async Task<Person> ActivatePersonAsync(int personId)
        {
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
                throw new ClientFaultException($"La persona con ID {personId} no existe.");

            person.Status = true;
            person.ModificationDate = DateTime.Now;
            _dbContext.Entry(person).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return person;
        }
    }
}
