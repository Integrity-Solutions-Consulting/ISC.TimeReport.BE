using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.DailyActivities;
using isc.time.report.be.application.Interfaces.Service.DailyActivities;
using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Models.Request.DailyActivities;
using isc.time.report.be.domain.Models.Response.DailyActivities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Services.DailyActivities
{
    public class DailyActivityService : IDailyActivityService
    {
        private readonly IDailyActivityRepository _repository;
        private readonly IMapper _mapper;

        public DailyActivityService(IDailyActivityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<GetDailyActivityResponse>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<List<GetDailyActivityResponse>>(list);
        }

        public async Task<GetDailyActivityResponse> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Actividad no encontrada");
            return _mapper.Map<GetDailyActivityResponse>(entity);
        }

        public async Task<CreateDailyActivityResponse> CreateAsync(CreateDailyActivityRequest request, int employeeId)
        {
            var entity = _mapper.Map<DailyActivity>(request);
            entity.EmployeeID = employeeId;
            var result = await _repository.CreateAsync(entity);
            return _mapper.Map<CreateDailyActivityResponse>(result);
        }

        public async Task<UpdateDailyActivityResponse> UpdateAsync(int id, UpdateDailyActivityRequest request, int employeeId)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Actividad no encontrada");

            _mapper.Map(request, entity);
            entity.EmployeeID = employeeId;

            var result = await _repository.UpdateAsync(entity);
            return _mapper.Map<UpdateDailyActivityResponse>(result);
        }

        public async Task<ActiveInactiveDailyActivityResponse> InactivateAsync(int id)
        {
            var result = await _repository.InactivateAsync(id);
            return _mapper.Map<ActiveInactiveDailyActivityResponse>(result);
        }

        public async Task<ActiveInactiveDailyActivityResponse> ActivateAsync(int id)
        {
            var result = await _repository.ActivateAsync(id);
            return _mapper.Map<ActiveInactiveDailyActivityResponse>(result);
        }
        public async Task<List<GetDailyActivityResponse>> ApproveActivitiesAsync(AproveDailyActivityRequest request, int approverId)
        {
            var result = await _repository.ApproveActivitiesAsync(request.ActivityId, request.EmployeeID, request.ProjectID, request.DateInicio, request.DateFin, approverId);
            return _mapper.Map<List<GetDailyActivityResponse>>(result);
        }
    }
}
