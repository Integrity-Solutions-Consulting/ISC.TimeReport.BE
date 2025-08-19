using AutoMapper;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Models.Request.Persons;
using isc.time.report.be.domain.Models.Response.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Utils.Mapping
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, CreatePersonRequest>();
            CreateMap<CreatePersonRequest, Person>();

            CreateMap<Person, UpdatePersonRequest>();
            CreateMap<UpdatePersonRequest, Person>();


            CreateMap<Person, GetPersonResponse>();
            CreateMap<GetPersonResponse, Person>();

            CreateMap<Person, CreatePersonResponse>();
            CreateMap<CreatePersonResponse, Person>();

            CreateMap<Person, UpdatePersonResponse>();
            CreateMap<UpdatePersonResponse, Person>();

            CreateMap<Person, ActiveInactivePersonResponse>();
            CreateMap<ActiveInactivePersonResponse, Person>();
        }
    }
}
