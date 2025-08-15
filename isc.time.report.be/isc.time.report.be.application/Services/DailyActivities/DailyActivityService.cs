using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.DailyActivities;
using isc.time.report.be.application.Interfaces.Repository.Permissions;
using isc.time.report.be.application.Interfaces.Repository.TimeReports;
using isc.time.report.be.application.Interfaces.Service.DailyActivities;
using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Exceptions;
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
        private readonly ITimeReportRepository _timeReportRepository;
        private readonly IPermissionRepository _permissionRepository;

        public DailyActivityService(IDailyActivityRepository repository, IMapper mapper, ITimeReportRepository timeReportRepository, IPermissionRepository permissionRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _timeReportRepository = timeReportRepository;
            _permissionRepository = permissionRepository;
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
            // Validar si la fecha es sábado o domingo
            var activityDate = request.ActivityDate;
            var dayOfWeek = activityDate.DayOfWeek;

            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                throw new InvalidOperationException("No se pueden registrar actividades los sábados o domingos.");

            // Validar si la fecha es feriado
            var holidays = await _timeReportRepository.GetActiveHolidaysByMonthAndYearAsync(activityDate.Month, activityDate.Year);

            bool esFeriado = holidays.Any(h =>
                (h.IsRecurring && h.HolidayDate.Day == activityDate.Day && h.HolidayDate.Month == activityDate.Month) ||
                (!h.IsRecurring && h.HolidayDate == activityDate));

            if (esFeriado)
                throw new InvalidOperationException("No se pueden registrar actividades en días feriados.");

            // Validar si la fecha está cubierta por un permiso aprobado
            var permisos = await _permissionRepository.GetPermissionsAprovedByEmployeeIdAsync(employeeId);

            bool enPermiso = permisos.Any(p =>
                activityDate.ToDateTime(TimeOnly.MinValue) >= p.StartDate.Date &&
                activityDate.ToDateTime(TimeOnly.MinValue) <= p.EndDate.Date);

            if (enPermiso)
                throw new InvalidOperationException("No se pueden registrar actividades durante un permiso aprobado.");

            //validar que las horas no sean negativas
            if (request.HoursQuantity < 0)
            {
                throw new ClientFaultException("No se puede ingresar horas negativas");
            }

            // Si todo está correcto, crear la actividad
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

            if (entity.EmployeeID != employeeId)
            {
                throw new ClientFaultException("El ID del Daily no pertenece al empleado");
            }

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
