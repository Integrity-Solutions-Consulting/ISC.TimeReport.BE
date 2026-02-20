using AutoMapper;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Models.Request.Employees;
using isc.time.report.be.domain.Models.Response.Employees;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, CreateEmployeeWithPersonIDRequest>();
            CreateMap<CreateEmployeeWithPersonIDRequest, Employee>();

            CreateMap<Employee, CreateEmployeeWithPersonOBJRequest>();
            CreateMap<CreateEmployeeWithPersonOBJRequest, Employee>();

            CreateMap<Employee, UpdateEmployeeWithPersonIDRequest>();
            CreateMap<UpdateEmployeeWithPersonIDRequest, Employee>();

            CreateMap<Employee, UpdateEmployeeWithPersonOBJRequest>();
            CreateMap<UpdateEmployeeWithPersonOBJRequest, Employee>();


            CreateMap<Employee, ActiveInactiveEmployeeResponse>();
            CreateMap<ActiveInactiveEmployeeResponse, Employee>();

            CreateMap<Employee, CreateEmployeeResponse>();
            CreateMap<CreateEmployeeResponse, Employee>();

            CreateMap<Employee, GetEmployeeDetailsResponse>();
            CreateMap<GetEmployeeDetailsResponse, Employee>();

            CreateMap<Employee, UpdateEmployeeResponse>();
            CreateMap<UpdateEmployeeResponse, Employee>();
        }
    }
}
