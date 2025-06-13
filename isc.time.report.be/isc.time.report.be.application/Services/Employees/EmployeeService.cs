using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.Employees;
using isc.time.report.be.application.Interfaces.Repository.Persons;
using isc.time.report.be.domain.Models.Response.Employees;
using entityPerson = isc.time.report.be.domain.Entity.Persons;
using entityEmployee = isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.application.Interfaces.Service.Employees;

namespace isc.time.report.be.application.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        public readonly IEmployeeRepository EmployeeRepository;
        public readonly IPersonRepository PersonRepository;

        public EmployeeService(IEmployeeRepository EmployeeRepository, IPersonRepository personRepository)
        {
            this.EmployeeRepository = EmployeeRepository;
            this.PersonRepository = personRepository;
        }

        public async Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest request)
        {
            var newPerson = new entityPerson.Person
            {
                GenderID = request.GenderId,
                NationalityId = request.NationalityId,
                IdentificationTypeId = request.IdentificationTypeId,
                IdentificationNumber = request.IdentificationNumber,
                PersonType = request.PersonType,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
            };

            await PersonRepository.CreatePerson(newPerson);

            var newEmployee = new entityEmployee.Employee
            {
                PersonID = newPerson.Id,
                PositionID = request.PositionID,
                EmployeeCode = request.EmployeeCode,
                HireDate = request.HireDate,
                TerminationDate = request.TerminationDate,
                ContractType = request.ContractType,
                Department = request.Department,
                CorporateEmail = request.CorporateEmail,
                Salary = request.Salary,
            };

            await EmployeeRepository.CreateEmployee(newEmployee);

            return new CreateEmployeeResponse();
        }
        public async Task<List<GetEmployeeResponse>> GetAll()
        {
            var employees = await EmployeeRepository.GetAllEmployees();
            
            return employees
                .Select(x => new GetEmployeeResponse 
                {
                    Id = x.Id,
                    GenderId = x.Person.GenderID,
                    NationalityId = x.Person.NationalityId,
                    IdentificationTypeId = x.Person.IdentificationTypeId,
                    IdentificationNumber = x.Person.IdentificationNumber,
                    PersonType = x.Person.PersonType,
                    FirstName = x.Person.FirstName,
                    LastName = x.Person.LastName,
                    BirthDate = x.Person.BirthDate,
                    Email = x.Person.Email,
                    Phone = x.Person.Phone,
                    Address = x.Person.Address,
                    PositionID = x.PositionID,
                    EmployeeCode = x.EmployeeCode,
                    HireDate = x.HireDate,
                    TerminationDate = x.TerminationDate,
                    ContractType = x.ContractType,
                    Department = x.Department,
                    CorporateEmail = x.CorporateEmail,
                    Salary = x.Salary,

                }).ToList();
        }
    }
}
