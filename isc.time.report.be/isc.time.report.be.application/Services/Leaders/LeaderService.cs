using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.application.Interfaces.Repository.Leaders;
using isc.time.report.be.application.Interfaces.Service.Leaders;
using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Models.Request.Leaders;
using isc.time.report.be.domain.Models.Response.Leaders;

namespace isc.time.report.be.application.Services.Leaders
{
    public class LeaderService : ILeaderService
    {
        public readonly ILeaderRepository leaderRepository;

        public LeaderService(ILeaderRepository leaderRepository)
        {
            this.leaderRepository = leaderRepository;
        }

        public async Task<CreateLeaderResponse> Create(CreateLeaderRequest createRequest)
        {
            var newLeader = new Leader
            {
                IdentificationType = createRequest.IdentificationType,
                IdentificationNumber = createRequest.IdentificationNumber,
                LeaderType = createRequest.LeaderType,
                Names = createRequest.Names,
                Surnames = createRequest.Surnames,
                Gender = createRequest.Gender,
                CellPhoneNumber = createRequest.CellPhoneNumber,
                Position = createRequest.Position,
                PersonalEmail = createRequest.PersonalEmail,
                CorporateEmail = createRequest.CorporateEmail,
                HomeAddress = createRequest.HomeAddress,
            };
            await leaderRepository.CreateLeader(newLeader);
            return new CreateLeaderResponse();
        }

        public async Task<List<GetLeaderListResponse>> GetAll()
        {
            var people = await leaderRepository.GetLeaders();

            return people.Select(c => new GetLeaderListResponse
            {
                Id = c.Id.ToString(),
                IdentificationType = c.IdentificationType,
                IdentificationNumber = c.IdentificationNumber,
                LeaderType = c.LeaderType,
                Names = c.Names,
                Surnames = c.Surnames,
                Gender = c.Gender,
                CellPhoneNumber = c.CellPhoneNumber,
                Position = c.Position,
                PersonalEmail = c.PersonalEmail,
                CorporateEmail = c.CorporateEmail,
                HomeAddress = c.HomeAddress,
            }).ToList();
        }
    }
}
