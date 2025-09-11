using AutoMapper;
using isc.time.report.be.application.Interfaces.Repository.Catalogs;
using isc.time.report.be.application.Interfaces.Repository.DailyActivities;
using isc.time.report.be.application.Interfaces.Repository.Employees;
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
        private readonly IEmployeeRepository _employeeRepository; 
        private readonly ICatalogRepository _catalogRepository;

        public DailyActivityService(IDailyActivityRepository repository, IMapper mapper, ITimeReportRepository timeReportRepository, IPermissionRepository permissionRepository, IEmployeeRepository employeeRepository, ICatalogRepository catalogRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _timeReportRepository = timeReportRepository;
            _permissionRepository = permissionRepository;
            _employeeRepository = employeeRepository;
            _catalogRepository = catalogRepository;
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

            //if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
            //    throw new InvalidOperationException("No se pueden registrar actividades los sábados o domingos.");

            // Validar si la fecha es feriado
            var holidays = await _timeReportRepository.GetActiveHolidaysByMonthAndYearAsync(activityDate.Month, activityDate.Year);

            bool esFeriado = holidays.Any(h =>
                (h.IsRecurring && h.HolidayDate.Day == activityDate.Day && h.HolidayDate.Month == activityDate.Month) ||
                (!h.IsRecurring && h.HolidayDate == activityDate));

            //if (esFeriado)
            //    throw new InvalidOperationException("No se pueden registrar actividades en dias feriados.");

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
            if (request.HoursQuantity == 0 || request.HoursQuantity == null)
            {
                throw new ClientFaultException("Las horas ingresadas no pueden ser 0");
            }

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

        public async Task<CreateListOfDailyActivityFromBG> ImportActivitiesAsync(List<CreateDailyActivityFromBGResponse> excelRows)
        {
            var results = new CreateListOfDailyActivityFromBG();

            // 1️⃣ Validar y mapear filas
            var activitiesToInsert = await MapAndValidateRowsAsync(excelRows, results);

            // 2️⃣ Insertar en bulk
            if (activitiesToInsert.Any())
            await _repository.AddRangeAsync(activitiesToInsert);

            return results;
        }

   

        private async Task<List<DailyActivity>> MapAndValidateRowsAsync(
                    List<CreateDailyActivityFromBGResponse> excelRows,
                    CreateListOfDailyActivityFromBG results)
        {
            var activities = new List<DailyActivity>();

            foreach (var row in excelRows)
            {
                try
                {
                    var activity = await MapSingleRowAsync(row);
                    activities.Add(activity);
                    row.Status = "Inserted";
                }
                catch (Exception ex)
                {
                    row.Status = $"Error: {ex.Message}";
                }

                results.Activities.Add(row);
            }

            return activities;
        }

        private async Task<DailyActivity> MapSingleRowAsync(CreateDailyActivityFromBGResponse row)
        {
            // 1️⃣ Employee
            var employee = await _employeeRepository.GetEmployeeByCodeAsync(row.EmployeeCode);
            string username = $"{employee.Person.FirstName} {employee.Person.LastName}";

            // 2️⃣ ActivityType
            var activityType = await _catalogRepository.GetActivityTypeByNameAsync(row.Type);
            if (activityType == null)
                throw new ClientFaultException($"ActivityType '{row.Type}' no encontrado");

            // 3️⃣ ActivityDescription
            string description = string.IsNullOrWhiteSpace(row.Comment) ? row.Title : row.Comment;

            // Validar horas
            if (!decimal.TryParse(row.Hours, out decimal hours) || hours < 0)
                throw new ClientFaultException("Horas inválidas");

            // Validar fecha y convertir a DateOnly
            var format = "d/M/yyyy H:mm:ss"; // Formato que trae el Excel
            var culture = System.Globalization.CultureInfo.InvariantCulture;

            if (!DateTime.TryParseExact(row.Date, format, culture, System.Globalization.DateTimeStyles.None, out DateTime activityDateTime))
            {
                throw new ClientFaultException("Fecha inválida");
            }

            var activityDate = DateOnly.FromDateTime(activityDateTime);

            // 5️⃣ Mapear a DailyActivity
            return new DailyActivity
            {
                EmployeeID = employee.Id,
                ActivityTypeID = activityType.Id,
                ActivityDate = activityDate, 
                HoursQuantity = hours,
                ActivityDescription = description,
                RequirementCode = row.RequirementCode,
                CreationUser = "SYSTEM",
                CreationDate = DateTime.Now,
                Status = true
            };

        }

    }
}
