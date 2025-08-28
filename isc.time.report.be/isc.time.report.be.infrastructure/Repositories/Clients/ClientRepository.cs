using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using isc.time.report.be.application.Interfaces.Repository.Clients;
using isc.time.report.be.application.Interfaces.Repository.InventoryApis;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Dto.InventorysApis.InventorysCustomers;
using isc.time.report.be.infrastructure.Database;
using isc.time.report.be.infrastructure.Repositories.InventorysApis;
using isc.time.report.be.infrastructure.Utils.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Clients
{
    public class ClientRepository : IClientRepository
    {
        private readonly DBContext _dbContext;
        private readonly InventoryApiRepository _inventoryApiRepository;

        public ClientRepository(DBContext dbContext, InventoryApiRepository inventoryApiRepository)
        {
            _dbContext = dbContext;
            _inventoryApiRepository = inventoryApiRepository;
        }

        public async Task<PagedResult<Client>> GetAllClientsPaginatedAsync(PaginationParams paginationParams, string? search)
        {
            var query = _dbContext.Clients
                .Include(c => c.Person)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string normalizedSearch = search.Trim().ToLower();

                query = query.Where(c =>
                    (c.TradeName != null && c.TradeName.ToLower().Contains(normalizedSearch)) ||
                    (c.LegalName != null && c.LegalName.ToLower().Contains(normalizedSearch)) ||
                    (c.Person != null && (
                        (c.Person.FirstName != null && c.Person.FirstName.ToLower().Contains(normalizedSearch)) ||
                        (c.Person.IdentificationNumber != null && c.Person.IdentificationNumber.ToLower().Contains(normalizedSearch)) ||
                        (c.Person.Email != null && c.Person.Email.ToLower().Contains(normalizedSearch)) ||
                        (c.Person.LastName != null && c.Person.LastName.ToLower().Contains(normalizedSearch))
                    )));                       
            }

            query = query.OrderBy(p => p.Status ? 0 : 1)
                .ThenBy(p => p.TradeName);



            return await PaginationHelper.CreatePagedResultAsync(query, paginationParams);
        }
        public async Task<PagedResult<Client>> GetClientsAssignedToEmployeeAsync(int employeeId, PaginationParams paginationParams, string? search)
        {
            // Obtenemos IDs de los clientes relacionados a proyectos asignados al empleado
            var clientIds = await _dbContext.EmployeeProjects
                .Where(ep => ep.EmployeeID == employeeId && ep.Project.Status == true && ep.Status == true)
                .Select(ep => ep.Project.ClientID)
                .Distinct()
                .ToListAsync();

            var query = _dbContext.Clients
                .Include(c => c.Person)
                .Where(c => clientIds.Contains(c.Id))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string normalizedSearch = search.Trim().ToLower();

                query = query.Where(c =>
                    (c.TradeName != null && c.TradeName.ToLower().Contains(normalizedSearch)) ||
                    (c.LegalName != null && c.LegalName.ToLower().Contains(normalizedSearch)) ||
                    (c.Person != null && (
                        (c.Person.FirstName != null && c.Person.FirstName.ToLower().Contains(normalizedSearch)) ||
                        (c.Person.IdentificationNumber != null && c.Person.IdentificationNumber.ToLower().Contains(normalizedSearch)) ||
                        (c.Person.Email != null && c.Person.Email.ToLower().Contains(normalizedSearch)) ||
                        (c.Person.LastName != null && c.Person.LastName.ToLower().Contains(normalizedSearch))
                    )));
            }

            return await PaginationHelper.CreatePagedResultAsync(query, paginationParams);
        }

        public async Task<Client> GetClientByIDAsync(int clientId)
        {
            if (clientId <= 0)
            {
                throw new ClientFaultException("El ID del rol no puede ser negativo");
            }
            return await _dbContext.Clients
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.Id == clientId);
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            client.CreationDate = DateTime.Now;
            client.ModificationDate = null;
            client.Status = true;

            await _dbContext.Clients.AddAsync(client);
            await _dbContext.SaveChangesAsync();

            return client;
        }

        public async Task<Client> CreateClientWithPersonAsync(Client client)
        {
            // Asume que client.Person viene poblado
            if (client.Person == null)
                throw new ClientFaultException("La entidad Person no puede ser nula.");

            client.Person.CreationDate = DateTime.Now;
            client.Person.Status = true;
            client.Person.CreationUser = "SYSTEM";

            client.CreationDate = DateTime.Now;
            client.Status = true;
            client.CreationUser = "SYSTEM";

            await _dbContext.Clients.AddAsync(client); // EF también agregará la Person asociada
            await _dbContext.SaveChangesAsync();

            return client;
        }

        //public async Task<Client> CreateClientWithPersonForInventoryAsync(Client client)
        //{
        //    if (client.Person == null)
        //        throw new ClientFaultException("La entidad Person no puede ser nula.");

        //    client.Person.CreationDate = DateTime.Now;
        //    client.Person.Status = true;
        //    client.Person.CreationUser = "SYSTEM";

        //    client.CreationDate = DateTime.Now;
        //    client.Status = true;
        //    client.CreationUser = "SYSTEM";

        //    var existingClient = await _dbContext.Clients
        //        .FirstOrDefaultAsync(p => p.Person.IdentificationNumber == client.Person.IdentificationNumber);

        //    if (existingClient != null)
        //    {
        //        throw new ClientFaultException($"Ya existe un cliente con ese numero de Identificacion '{client.Person.IdentificationNumber}'.");
        //    }


        //    using var transaction = await _dbContext.Database.BeginTransactionAsync();

        //    try
        //    {
        //        await _dbContext.Clients.AddAsync(client);

        //        // Enviar al inventario
        //        var inventoryRequest = new InventoryCreateCustomerRequest
        //        {
        //            Name = client.TradeName ?? "No definido",
        //            Address = client.Person.Address ?? "No definido",
        //            Email = client.Person.Email ?? "nodefinido@example.com",
        //            Phone = client.Person.Phone ?? "000000000",
        //            Ruc = client.Person.IdentificationNumber
        //        };

        //        var result = await _inventoryApiRepository.CreateCustomerInventoryAsync(inventoryRequest);
        //        if (!result)
        //            throw new ClientFaultException("No se pudo registrar el cliente en el inventario.");

        //        await _dbContext.SaveChangesAsync();
        //        await transaction.CommitAsync();
        //        return client;
        //    }
        //    catch
        //    {
        //        await transaction.RollbackAsync();
        //        throw;
        //    }
        //}

        public async Task<Client> CreateClientWithPersonForInventoryAsync(Client client)
        {
            if (client.Person == null)
                throw new ClientFaultException("La entidad Person no puede ser nula.");

            // Validación duplicados
            var existingClient = await _dbContext.Clients
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.Person.IdentificationNumber == client.Person.IdentificationNumber);

            if (existingClient != null)
                throw new ClientFaultException(
                    $"Ya existe un cliente con ese número de identificación '{client.Person.IdentificationNumber}'."
                );

            client.Person.CreationDate = DateTime.Now;
            client.Person.Status = true;
            client.Person.CreationUser = "SYSTEM";

            client.CreationDate = DateTime.Now;
            client.Status = true;
            client.CreationUser = "SYSTEM";

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.Clients.AddAsync(client);
                await _dbContext.SaveChangesAsync(); // Inserción local

                // Enviar a inventario
                var inventoryRequest = new InventoryCreateCustomerRequest
                {

                    Id = client.Id,
                    Name = client.TradeName ?? "No definido",
                    Address = client.Person.Address ?? "No definido",
                    Email = client.Person.Email ?? "nodefinido@example.com",
                    Phone = client.Person.Phone ?? "000000000",
                    Ruc = client.Person.IdentificationNumber
                };

                var result = await _inventoryApiRepository.CreateCustomerInventoryAsync(inventoryRequest);
                if (!result)
                    throw new ClientFaultException("No se pudo registrar el cliente en el inventario.");

                await transaction.CommitAsync();
                return client;
            }
            catch
            {
                await transaction.RollbackAsync(); // Revierte la inserción local si falla API
                throw;
            }
        }


        public async Task<Client> UpdateClientAsync(Client client)
        {
            client.ModificationDate = DateTime.Now;
            _dbContext.Entry(client).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return client;
        }

        public async Task<Client> UpdateClientWithPersonAsync(Client client)
        {
            if (client == null || client.Person == null)
                throw new ClientFaultException("El cliente o su persona asociada no pueden ser nulos.");

            var existingClient = await _dbContext.Clients
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.Id == client.Id);

            if (existingClient == null)
                throw new ClientFaultException($"No existe el cliente con ID {client.Id}");


            if (client.Person.Id != existingClient.Person.Id)
                throw new ClientFaultException("La persona ingresada, no corrsponde al ciente");

            client.Person.ModificationDate = DateTime.Now;
            client.Person.ModificationUser = "SYSTEM";

            _dbContext.Entry(existingClient.Person).CurrentValues.SetValues(client.Person);
            _dbContext.Entry(existingClient.Person).State = EntityState.Modified;

            client.ModificationDate = DateTime.Now;
            client.ModificationUser = "SYSTEM";

            _dbContext.Entry(existingClient).CurrentValues.SetValues(client);
            _dbContext.Entry(existingClient).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return existingClient;
        }

        public async Task<Client> UpdateClientWithPersonForInventoryAsync(string identification, Client client)
        {
            if (client == null || client.Person == null)
                throw new ClientFaultException("El cliente o su persona asociada no pueden ser nulos.");

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var existingClient = await _dbContext.Clients
                    .Include(c => c.Person)
                    .FirstOrDefaultAsync(c => c.Id == client.Id);

                if (existingClient == null)
                    throw new ClientFaultException($"No existe el cliente con ID {client.Id}");

                if (client.Person.Id != existingClient.Person.Id)
                    throw new ClientFaultException("La persona ingresada no corresponde al cliente.");

                existingClient.Person.IdentificationNumber = client.Person.IdentificationNumber;
                existingClient.Person.PersonType = client.Person.PersonType;
                existingClient.Person.FirstName = client.Person.FirstName;
                existingClient.Person.LastName = client.Person.LastName;
                existingClient.Person.BirthDate = client.Person.BirthDate;
                existingClient.Person.Email = client.Person.Email;
                existingClient.Person.Phone = client.Person.Phone;
                existingClient.Person.Address = client.Person.Address;
                existingClient.Person.GenderID = client.Person.GenderID;
                existingClient.Person.NationalityId = client.Person.NationalityId;
                existingClient.Person.IdentificationTypeId = client.Person.IdentificationTypeId;
                existingClient.Person.ModificationDate = DateTime.Now;
                existingClient.Person.ModificationUser = "SYSTEM";

                existingClient.TradeName = client.TradeName;
                existingClient.LegalName = client.LegalName;
                existingClient.Company = client.Company;
                existingClient.ModificationDate = DateTime.Now;
                existingClient.ModificationUser = "SYSTEM";

                _dbContext.Entry(existingClient.Person).State = EntityState.Modified;
                _dbContext.Entry(existingClient).State = EntityState.Modified;

                await _dbContext.SaveChangesAsync();

                var updateRequest = new InventoryUpdateCustomerRequest
                {
                    name = existingClient.TradeName ?? "No definido",
                    address = existingClient.Person.Address ?? "No definido",
                    email = existingClient.Person.Email ?? "noreply@example.com",
                    phone = existingClient.Person.Phone ?? "000000000",
                    ruc = existingClient.Person.IdentificationNumber ?? "000000000"
                };

                var success = await _inventoryApiRepository.UpdateCustomerInventoryAsync(updateRequest, identification);
                if (!success)
                    throw new ClientFaultException("No se pudo actualizar el cliente en el inventario.");

                await transaction.CommitAsync();
                return existingClient;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Client> InactivateClientAsync(int clientId)
        {
            var client = await _dbContext.Clients.Include(c => c.Person).FirstOrDefaultAsync(c => c.Id == clientId);
            if (client == null)
                throw new ClientFaultException($"El cliente con ID {clientId} no existe.");

            client.Status = false;
            client.ModificationDate = DateTime.Now;
            _dbContext.Entry(client).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return client;
        }

        public async Task<Client> InactivateClientForInventoryAsync(int clientId)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var client = await _dbContext.Clients.Include(c => c.Person).FirstOrDefaultAsync(c => c.Id == clientId);
            if (client == null)
                throw new ClientFaultException($"El cliente con ID {clientId} no existe.");
            try
            {
                client.Status = false;
                client.ModificationDate = DateTime.Now;
                _dbContext.Entry(client).State = EntityState.Modified;

                await _dbContext.SaveChangesAsync();

                var result = await _inventoryApiRepository.InactivateCustomerInventoryAsync(client.Person.IdentificationNumber);
                if (!result)
                    throw new InvalidOperationException("No se pudo desactivar el cliente en inventario.");

                await transaction.CommitAsync();
                return client;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Client> ActivateClientAsync(int clientId)
        {
            var client = await _dbContext.Clients.Include(c => c.Person).FirstOrDefaultAsync(c => c.Id == clientId);
            if (client == null)
                throw new ClientFaultException($"El cliente con ID {clientId} no existe.");

            client.Status = true;
            client.ModificationDate = DateTime.Now;
            _dbContext.Entry(client).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return client;
        }

        public async Task<Client> ActivateClientForInventoryAsync(int clientId)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var client = await _dbContext.Clients.Include(c => c.Person).FirstOrDefaultAsync(c => c.Id == clientId);
            if (client == null)
                throw new ClientFaultException($"El cliente con ID {clientId} no existe.");

            try
            {
                client.Status = true;
                client.ModificationDate = DateTime.Now;
                _dbContext.Entry(client).State = EntityState.Modified;

                await _dbContext.SaveChangesAsync();

                var result = await _inventoryApiRepository.ActivateCustomerInventoryAsync(client.Person.IdentificationNumber);
                if (!result)
                    throw new InvalidOperationException("No se pudo activar el cliente en inventario.");

                await transaction.CommitAsync();
                return client;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Client>> GetClientsByEmployeeIdAsync(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ClientFaultException("El ID del empleado no puede ser menor a 0.");
            }

            var clients =  await _dbContext.EmployeeProjects
                .Where(ep => ep.EmployeeID == employeeId)
                .Select(ep => ep.Project.Client)
                .Distinct()
                .Include(c => c.Person)
                .ToListAsync();
            return clients;
        }

        public async Task<Person?> ValidateUNIQUEIdentificationNumberAsync(string identificationNumber)
        {
            var person = await _dbContext.Persons
                .FirstOrDefaultAsync(c => c.IdentificationNumber == identificationNumber);

            return person;
        }
    }
}
