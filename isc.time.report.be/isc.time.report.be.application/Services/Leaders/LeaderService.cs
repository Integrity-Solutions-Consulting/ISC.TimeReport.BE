using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Leaders;
using isc.time.report.be.application.Interfaces.Service.Leaders;
using isc.time.report.be.domain.Entity.Leaders;
using entityPerson = isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Request.Persons;

namespace isc.time.report.be.application.Services.Leaders
{
    public class LeaderService : ILeaderService
    {
        public readonly ILeaderRepository leaderRepository;

        public LeaderService(ILeaderRepository leaderRepository)
        {
            this.leaderRepository = leaderRepository;
        }

        public async Task<CreateLeaderWithPersonResponse> Create(CreateLeaderWithPersonRequest createRequest)
        {
            var newPerson = new entityPerson.Person
            {
                IdentificationType = createRequest.IdentificationType,
                IdentificationNumber = createRequest.IdentificationNumber,
                Names = createRequest.Names,
                Surnames = createRequest.Surnames,
                Gender = createRequest.Gender,
                CellPhoneNumber = createRequest.CellPhoneNumber,
                Position = createRequest.Position,
                PersonalEmail = createRequest.PersonalEmail,
                CorporateEmail = createRequest.CorporateEmail,
                HomeAddress = createRequest.HomeAddress,
            };

            var newLeader = new Leader
            {
                LeaderType = createRequest.LeaderType,
                ProjectCode = createRequest.ProjectCode,
                CustomerCode = createRequest.CustomerCode,
                Person = newPerson
            };
            await leaderRepository.CreateLeader(newLeader);
            return new CreateLeaderWithPersonResponse();
        }

        public async Task<List<GetLeaderListResponse>> GetAll()
        {
            var leader = await leaderRepository.GetLeaders();

            return leader
                .Select(l => new GetLeaderListResponse
                {

                    Id = l.Id.ToString(),
                    LeaderType = l.LeaderType,
                    ProjectCode = l.ProjectCode,
                    CustomerCode = l.CustomerCode,
                    IdentificationType = l.Person.IdentificationType,
                    IdentificationNumber = l.Person.IdentificationNumber,
                    Names = l.Person.Names,
                    Surnames = l.Person.Surnames,
                    Gender = l.Person.Gender,
                    CellPhoneNumber = l.Person.CellPhoneNumber,
                    Position = l.Person.Position,
                    PersonalEmail = l.Person.PersonalEmail,
                    CorporateEmail = l.Person.CorporateEmail,
                    HomeAddress = l.Person.HomeAddress,

                }).ToList();
            
        }

        public async Task<UpdateLeaderResponse> Update(UpdateLeaderRequest request)
        {
            var leader = await leaderRepository.GetLeaderById(request.Id);

            if (leader == null)
            {
                return new UpdateLeaderResponse
                {
                    Success = false,
                    Message = "Líder no Encontrado"
                };
            }
            leader.LeaderType = request.LeaderType;
            leader.ProjectCode = request.ProjectCode;
            leader.CustomerCode = request.CustomerCode;

            if(leader.Person != null)
            {
                leader.Person.IdentificationType = request.IdentificationType;
                leader.Person.IdentificationNumber = request.IdentificationNumber;
                leader.Person.Names = request.Names;
                leader.Person.Surnames = request.Surnames;
                leader.Person.Gender = request.Gender;
                leader.Person.CellPhoneNumber = request.CellPhoneNumber;
                leader.Person.Position = request.Position;
                leader.Person.PersonalEmail = request.PersonalEmail;
                leader.Person.CorporateEmail = request.CorporateEmail;
                leader.Person.HomeAddress = request.HomeAddress;
            }

            await leaderRepository.UpdateLeader(leader);

            return new UpdateLeaderResponse
            {
                Success = true,
                Message = "Líder actualizado"
            };
        }
    }
}
