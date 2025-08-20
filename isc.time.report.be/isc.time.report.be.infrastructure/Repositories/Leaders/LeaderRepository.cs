using DocumentFormat.OpenXml.Office2010.Excel;
using isc.time.report.be.application.Interfaces.Repository.Leaders;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Utils.Pagination;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Leaders
{
    public class LeaderRepository : ILeaderRepository
    {
        private readonly DBContext _dbContext;

        public LeaderRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<Leader>> GetAllLeadersPaginatedAsync(PaginationParams paginationParams, string? search)
        {
            var query = _dbContext.Leaders
                .Include(e => e.Person)
                .Include(e => e.Project)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string normalizedSearch = search.Trim();

                query = query.Where(l =>
                    (l.Person != null && (
                        (l.Person.FirstName != null && l.Person.FirstName.Contains(normalizedSearch)) ||
                        (l.Person.LastName != null && l.Person.LastName.Contains(normalizedSearch)) ||
                        (l.Person.IdentificationNumber != null && l.Person.IdentificationNumber.Contains(normalizedSearch))
                    )) ||
                    (l.Responsibilities != null && l.Responsibilities.Contains(normalizedSearch)) ||
                    (l.Project != null && l.Project.Name != null && l.Project.Name.Contains(normalizedSearch))
                );
            }

            return await PaginationHelper.CreatePagedResultAsync(query, paginationParams);
        }

        public async Task<Leader> GetLeaderByIDAsync(int leaderId)
        {
            if (leaderId <= 0)
            {
                throw new ClientFaultException("El ID de la Leader no puede ser negativo");
            }
            var leader = await _dbContext.Leaders
                .Include(e => e.Person)
                .Include(e => e.Project)
                .FirstOrDefaultAsync(e => e.Id == leaderId);
            if (leader == null)
            {
                throw new ClientFaultException($"No se encontró un líder con ID {leaderId}.");
            }
            return leader;
        }

        public async Task<Leader> CreateLeaderAsync(Leader leader)
        {
            leader.CreationDate = DateTime.Now;
            leader.ModificationDate = null;
            leader.Status = true;

            await _dbContext.Leaders.AddAsync(leader);
            await _dbContext.SaveChangesAsync();

            return leader;
        }

        public async Task<Leader> CreateLeaderWithPersonAsync(Leader leader)
        {
            if (leader.Person == null)
                throw new ClientFaultException("La entidad Person no puede ser nula.");

            leader.Person.CreationDate = DateTime.Now;
            leader.Person.Status = true;
            leader.Person.CreationUser = "SYSTEM";

            leader.CreationDate = DateTime.Now;
            leader.Status = true;
            leader.CreationUser = "SYSTEM";

            try
            {
                await _dbContext.Leaders.AddAsync(leader);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;

                Console.WriteLine($"Error al guardar en base de datos: {innerMessage}");

                throw new Exception($"Error al guardar los datos: {innerMessage}", ex);
            }

            return leader;
        }

        public async Task<Leader> UpdateLeaderAsync(Leader leader)
        {
            leader.ModificationDate = DateTime.Now;
            _dbContext.Entry(leader).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return leader;
        }

        public async Task<Leader> UpdateLeaderWithPersonAsync(Leader leader)
        {
            if (leader == null || leader.Person == null)
                throw new ClientFaultException("El líder o su persona asociada no pueden ser nulos.");

            var existingLeader = await _dbContext.Leaders
                .Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.Id == leader.Id);

            if (existingLeader == null)
                throw new ClientFaultException($"No existe el líder con ID {leader.Id}");

            if (leader.Person.Id != existingLeader.Person.Id)
                throw new ClientFaultException("La persona ingresada no corresponde al líder");

            leader.Person.ModificationDate = DateTime.Now;
            leader.Person.ModificationUser = "SYSTEM";

            _dbContext.Entry(existingLeader.Person).CurrentValues.SetValues(leader.Person);
            _dbContext.Entry(existingLeader.Person).State = EntityState.Modified;

            leader.ModificationDate = DateTime.Now;
            leader.ModificationUser = "SYSTEM";

            _dbContext.Entry(existingLeader).CurrentValues.SetValues(leader);
            _dbContext.Entry(existingLeader).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return existingLeader;
        }

        public async Task<Leader> InactivateLeaderAsync(int leaderId)
        {
            var leader = await _dbContext.Leaders.Include(e => e.Person).FirstOrDefaultAsync(e => e.Id == leaderId);
            if (leader == null)
                throw new ClientFaultException($"El líder con ID {leaderId} no existe.");

            leader.Status = false;
            leader.ModificationDate = DateTime.Now;
            _dbContext.Entry(leader).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return leader;
        }

        public async Task<Leader> ActivateLeaderAsync(int leaderId)
        {
            var leader = await _dbContext.Leaders.Include(e => e.Person).FirstOrDefaultAsync(e => e.Id == leaderId);
            if (leader == null)
                throw new ClientFaultException($"El líder con ID {leaderId} no existe.");

            leader.Status = true;
            leader.ModificationDate = DateTime.Now;
            _dbContext.Entry(leader).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return leader;
        }

        public async Task<List<Leader>> GetActiveLeadersByProjectIdsAsync(List<int> projectIds)
        {
            if (projectIds == null || !projectIds.Any())
            {
                throw new ServerFaultException("No se proporcionaron IDs de proyectos válidos.");
            }
            var today = DateOnly.FromDateTime(DateTime.Today);

            var leaders = await _dbContext.Leaders
                .Include(l => l.Person)
                .Where(l => projectIds.Contains(l.ProjectID) &&
                            (l.EndDate == null || l.EndDate > today))
                .ToListAsync();
            if (!leaders.Any())
            {
                throw new ServerFaultException("No se encontraron líderes activos para los proyectos indicados.");
            }
            return leaders;
        }


        public async Task<List<Leader>> GetAllLeaderProjectByPersonIdAsync()
        {
            var list = await _dbContext.Leaders
                .Include(l => l.Person)     
                .ToListAsync();

            return list;
        }
        public async Task<List<Leader>> GetLeaderProjectByIdAsync (int personId)
        {

            var list = await _dbContext.Leaders
               .Where(le => le.PersonID == personId)

               .ToListAsync();

            return list;

        }

        public async Task<Person> GetLeadershipByPersonId (int personid)
        {
            var leadership = await _dbContext.Persons
                .Include(e => e.Leader)
                    .ThenInclude(l => l.Project)
                //.Distinct()
                .FirstOrDefaultAsync(e => e.Id == personid);
            return leadership;

        }

    }
}
