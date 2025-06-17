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
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Response.Persons;

namespace isc.time.report.be.application.Services.Leaders
{
    public class LeaderService : ILeaderService
    {
        public readonly ILeaderRepository leaderRepository;

        public LeaderService(ILeaderRepository leaderRepository)
        {
            this.leaderRepository = leaderRepository;
        }

        /* public async Task<CreateLeaderWithPersonResponse> Create(CreateLeaderWithPersonRequest createRequest)
        {
            var newPerson = new entityPerson.Persons
            {
                IdentificationTypeId = createRequest.IdentificationType,
                IdentificationNumber = createRequest.IdentificationNumber,
                FirstName = createRequest.Names,
                LastName = createRequest.Surnames,
                Gender = createRequest.Gender,
                CellPhoneNumber = createRequest.CellPhoneNumber,
                Position = createRequest.Position,
                PersonalEmail = createRequest.PersonalEmail,
                CorporateEmail = createRequest.CorporateEmail,
                Address = createRequest.HomeAddress,
            };

            var newLeader = new Leader
            {
                LeaderType = createRequest.LeaderType,
                ProjectCode = createRequest.ProjectCode,
                CustomerCode = createRequest.CustomerCode,
                Persons = newPerson
            };
            await leaderRepository.CreateLeader(newLeader);
            return new CreateLeaderWithPersonResponse();
        } */

        /*public async Task<List<GetLeaderListResponse>> GetAll()
        {
            var leader = await leaderRepository.GetLeaders();

            return leader
                .Select(l => new GetLeaderListResponse
                {

                    Id = l.Id.ToString(),
                    LeaderType = l.LeaderType,
                    ProjectCode = l.ProjectCode,
                    CustomerCode = l.CustomerCode,
                    IdentificationType = l.Persons.IdentificationTypeId,
                    IdentificationNumber = l.Persons.IdentificationNumber,
                    Names = l.Persons.FirstName,
                    Surnames = l.Persons.LastName,
                    Gender = l.Persons.Gender,
                    CellPhoneNumber = l.Persons.CellPhoneNumber,
                    Position = l.Persons.Position,
                    PersonalEmail = l.Persons.PersonalEmail,
                    CorporateEmail = l.Persons.CorporateEmail,
                    HomeAddress = l.Persons.Address,

                }).ToList();
            
        }*/

        /*public async Task<UpdateLeaderResponse> Update(UpdateLeaderRequest request)
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

            if(leader.Persons != null)
            {
                leader.Persons.IdentificationTypeId = request.IdentificationType;
                leader.Persons.IdentificationNumber = request.IdentificationNumber;
                leader.Persons.FirstName = request.Names;
                leader.Persons.LastName = request.Surnames;
                leader.Persons.Gender = request.Gender;
                leader.Persons.CellPhoneNumber = request.CellPhoneNumber;
                leader.Persons.Position = request.Position;
                leader.Persons.PersonalEmail = request.PersonalEmail;
                leader.Persons.CorporateEmail = request.CorporateEmail;
                leader.Persons.Address = request.HomeAddress;
            }

            await leaderRepository.UpdateLeader(leader);

            return new UpdateLeaderResponse
            {
                Success = true,
                Message = "Líder actualizado"
            };
        }*/
    }
}
