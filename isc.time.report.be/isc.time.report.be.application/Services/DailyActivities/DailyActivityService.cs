using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using isc.time.report.be.application.Interfaces.Repository.Catalogs;
using isc.time.report.be.application.Interfaces.Repository.DailyActivities;
using isc.time.report.be.application.Interfaces.Repository.Employees;
using isc.time.report.be.application.Interfaces.Repository.Permissions;
using isc.time.report.be.application.Interfaces.Repository.TimeReports;
using isc.time.report.be.application.Interfaces.Service.DailyActivities;
using isc.time.report.be.application.Utils.Auth;
using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.DailyActivities;
using isc.time.report.be.domain.Models.Response.Clients;
using isc.time.report.be.domain.Models.Response.DailyActivities;
using Microsoft.AspNetCore.Mvc;
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
        private readonly JWTUtils _jwtUtils;


        public DailyActivityService(IDailyActivityRepository repository, IMapper mapper, ITimeReportRepository timeReportRepository, IPermissionRepository permissionRepository, IEmployeeRepository employeeRepository, ICatalogRepository catalogRepository, JWTUtils jwtUtils)
        {
            _repository = repository;
            _mapper = mapper;
            _timeReportRepository = timeReportRepository;
            _permissionRepository = permissionRepository;
            _employeeRepository = employeeRepository;
            _catalogRepository = catalogRepository;
            _jwtUtils = jwtUtils;
        }

        public async Task<List<GetDailyActivityResponse>> GetAllAsync(int employeeId, int month, int year)
        {
            var result = await _repository.GetAllAsync(employeeId, month, year);

            var mapped = _mapper.Map<List<GetDailyActivityResponse>>(result);

            return mapped;
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
            // 🔹 Validar si el mes ya fue aprobado
            bool mesAprobado = await _repository.ExistsApprovedActivitiesAsync(employeeId, activityDate.Month, activityDate.Year);
            if (mesAprobado)
                throw new InvalidOperationException("No se pueden registrar actividades porque el mes ya ha sido aprobado.");

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
        public async Task ValidateActivityIsEditableAsync(int activityId)
        {
            var entity = await _repository.GetByIdAsync(activityId);

            if (entity.ApprovedByID != null)
            {
                throw new ClientFaultException("La actividad ya fue aprobada y no puede ser modificada");
            }

        }

        public async Task<UpdateDailyActivityResponse> UpdateAsync(int id, UpdateDailyActivityRequest request, int employeeId)
        {
            if (request.HoursQuantity == 0 || request.HoursQuantity == null)
            {
                throw new ClientFaultException("Las horas ingresadas no pueden ser 0");
            }

            await ValidateActivityIsEditableAsync(id);

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
            await ValidateActivityIsEditableAsync(id);
            var result = await _repository.InactivateAsync(id);
            return _mapper.Map<ActiveInactiveDailyActivityResponse>(result);
        }

        public async Task<ActiveInactiveDailyActivityResponse> ActivateAsync(int id)
        {
            await ValidateActivityIsEditableAsync(id);
            var result = await _repository.ActivateAsync(id);
            return _mapper.Map<ActiveInactiveDailyActivityResponse>(result);
        }
        public async Task<List<GetDailyActivityResponse>> ApproveActivitiesAsync(
         AproveDailyActivityRequest request,
         int approverId)
        {
            // Calcula primer y último día del mes
            var firstDayOfMonth = new DateTime(request.Year, request.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //  Actualiza actividades
            await _repository.ApproveActivitiesAsync(
                request.ActivityId,
                request.EmployeeID,
                request.ProjectID,
                firstDayOfMonth,
                lastDayOfMonth,
                approverId
            );

            //  Trae actividades actualizadas
            var updatedActivities = await _repository.GetActivitiesForApprovalAsync(
                request.ActivityId,
                request.EmployeeID,
                request.ProjectID,
                firstDayOfMonth,
                lastDayOfMonth
            );

            //  Mapea y retorna
            return _mapper.Map<List<GetDailyActivityResponse>>(updatedActivities);
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
            // 1 Employee
            var employee = await _employeeRepository.GetEmployeeByCodeAsync(row.EmployeeCode);

            var projectIdVerify = await _employeeRepository.GetProjectIdForEmployeeAsync(row.EmployeeCode?.Trim());

            if (projectIdVerify == null)
                throw new ClientFaultException($"El empleado {row.Username} no está asignado a proyectos del Cliente Banco Guayaquil.");


            // 2 ActivityType
            var activityType = await _catalogRepository.GetActivityTypeByNameAsync(row.Type);
            if (activityType == null)
                throw new ClientFaultException($"ActivityType '{row.Type}' no encontrado");

            
            // 3 ActivityDescription
            if (string.IsNullOrWhiteSpace(row.Title) && string.IsNullOrWhiteSpace(row.Comment))
                throw new ClientFaultException("Debe existir al menos un Título o un Comentario", 400);

            string description = string.IsNullOrWhiteSpace(row.Comment) ? row.Title : row.Comment;

            // 4 Validar horas
            if (!decimal.TryParse(row.Hours, out decimal hours) || hours < 0)
                throw new ClientFaultException("Horas inválidas");

            // 5 Validar fecha y convertir a DateOnly
            DateTime activityDateTime;

            // 1 Intentar como número serial de Excel
            if (double.TryParse(row.Date, System.Globalization.NumberStyles.Any,
                                System.Globalization.CultureInfo.InvariantCulture, out double oaDate))
            {
                activityDateTime = DateTime.FromOADate(oaDate);
            }
            else
            {
                // 2️ Intentar como texto con formato de fecha
                var format = "d/M/yyyy H:mm:ss"; // o "d/M/yyyy H:mm" si Excel no trae segundos
                var culture = System.Globalization.CultureInfo.InvariantCulture;

                if (!DateTime.TryParseExact(row.Date, format, culture,
                    System.Globalization.DateTimeStyles.None, out activityDateTime))
                {
                    throw new ClientFaultException("Fecha inválida, formato esperado d/M/yyyy H:mm:ss");
                }
            }

            // 3 Convertir a DateOnly para la base
            var activityDate = DateOnly.FromDateTime(activityDateTime);




            // 6️ Obtener ProjectID desde EmployeeProject (solo Banco Guayaquil)
            var projectId = await _employeeRepository.GetProjectIdForEmployeeAsync(row.EmployeeCode);
            if (projectId == null)
                throw new ClientFaultException($"El empleado {row.EmployeeCode} no tiene proyecto asignado para Banco Guayaquil");

            // 7 Mapear a DailyActivity
            return new DailyActivity
            {
                EmployeeID = employee.Id,
                ProjectID = projectId.Value,        
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


        public async Task<List<CreateDailyActivityFromBGResponse>> ReadActivitiesFromExcelAsync(Stream fileStream)
        {
            var rowsList = new List<CreateDailyActivityFromBGResponse>();

            using var document = SpreadsheetDocument.Open(fileStream, false);
            var workbookPart = document.WorkbookPart;
            var sheet = workbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
            var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id!);
            var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();


            // Leer todas las filas, excepto la de encabezado, y filtrar filas vacías
            var rows = sheetData.Elements<Row>()
                                .Skip(1)
                                .Where(r => r.Elements<Cell>()
                                             .Any(c => !string.IsNullOrWhiteSpace(GetCellValue(c, workbookPart))));


            foreach (var row in rows)
            {
                var cells = row.Elements<Cell>().ToList();

                // Asegurar que siempre haya 8 columnas (rellenar si faltan)
                while (cells.Count < 8)
                    cells.Add(new Cell());

                rowsList.Add(new CreateDailyActivityFromBGResponse
                {
                    Type = GetCellValue(cells[0], workbookPart),
                    Title = GetCellValue(cells[1], workbookPart),
                    RequirementCode = GetCellValue(cells[2], workbookPart),
                    Date = GetCellValue(cells[3], workbookPart),
                    Username = GetCellValue(cells[4], workbookPart),
                    Hours = GetCellValue(cells[5], workbookPart),
                    EmployeeCode = GetCellValue(cells[6], workbookPart),
                    Comment = GetCellValue(cells[7], workbookPart)
                });

            }

            return rowsList;
        }

        private string GetCellValue(Cell cell, WorkbookPart workbookPart)
        {
            if (cell.CellValue == null) return string.Empty;

            string value = cell.CellValue.Text;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return workbookPart.SharedStringTablePart!.SharedStringTable
                       .Elements<SharedStringItem>()
                       .ElementAt(int.Parse(value))
                       .InnerText;
            }

            return value;
        }
    }

}

