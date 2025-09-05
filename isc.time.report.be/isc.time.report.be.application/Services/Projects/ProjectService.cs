using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Repository.Clients;
using isc.time.report.be.application.Interfaces.Repository.Leaders;
using isc.time.report.be.application.Interfaces.Repository.Menus;
using isc.time.report.be.application.Interfaces.Repository.Projects;
using isc.time.report.be.application.Interfaces.Service.Projects;
using isc.time.report.be.application.Utils.Auth;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Projects;
using isc.time.report.be.domain.Entity.Shared;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Projects;
using isc.time.report.be.domain.Models.Response.Auth;
using isc.time.report.be.domain.Models.Response.Employees;
using isc.time.report.be.domain.Models.Response.Leaders;
using isc.time.report.be.domain.Models.Response.Persons;
using isc.time.report.be.domain.Models.Response.Projects;
using isc.time.report.be.domain.Models.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository projectRepository;
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;
        private readonly ILeaderRepository _leaderRepository;
        public ProjectService(IProjectRepository projectRepository, IMapper mapper, IClientRepository clientRepository, ILeaderRepository leaderRepository)
        {
            this.projectRepository = projectRepository;
            _mapper = mapper;
            _clientRepository = clientRepository;
            _leaderRepository = leaderRepository;
        }

        public async Task<PagedResult<GetAllProjectsResponse>> GetAllProjectsPaginated(PaginationParams paginationParams, string? search)
        {
            var result = await projectRepository.GetAllProjectsPaginatedAsync(paginationParams, search);

            var responseItems = _mapper.Map<List<GetAllProjectsResponse>>(result.Items);

            //foreach (var project in result) 

            return new PagedResult<GetAllProjectsResponse>
            {
                Items = responseItems,
                TotalItems = result.TotalItems,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        public async Task<PagedResult<GetAllProjectsResponse>> GetAllProjectsByEmployeeIDPaginated(
            PaginationParams paginationParams,
            string? search,
            int employeeId)
        {
            // Obtiene solo los proyectos asignados al empleado
            var result = await projectRepository.GetAssignedProjectsForEmployeeAsync(paginationParams, search, employeeId);

            var responseItems = _mapper.Map<List<GetAllProjectsResponse>>(result.Items);

            return new PagedResult<GetAllProjectsResponse>
            {
                Items = responseItems,
                TotalItems = result.TotalItems,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        public async Task<GetProjectByIDResponse> GetProjectByID(int projectID)
        {
            var project = await projectRepository.GetProjectByIDAsync(projectID);

            if (project == null)
            {
                return null;
            }

            var responseProject = _mapper.Map<GetProjectByIDResponse>(project);

            return responseProject;
        }

        public async Task<CreateProjectResponse> CreateProject(CreateProjectRequest projectRequest)
        {


            var projectNew = await projectRepository.CreateProject(_mapper.Map<Project>(projectRequest));

            if (projectNew.StartDate > projectNew.EndDate)
            {
                throw new ClientFaultException("No puede ingresar una fecha de Fin anterior a la fecha de Inicio.", 401);
            }

            return _mapper.Map<CreateProjectResponse>(projectNew);
        }

        public async Task<UpdateProjectResponse> UpdateProject(int projectId, UpdateProjectRequest projectParaUpdate)
        {
            var projectGet = await projectRepository.GetProjectByIDAsync(projectId);

            if (projectGet == null)
            {
                throw new ClientFaultException("No existe el proyecto", 401);
            }

            projectGet.ClientID = projectParaUpdate.ClientID;
            projectGet.ProjectStatusID = projectParaUpdate.ProjectStatusID;
            projectGet.ProjectTypeID = projectParaUpdate.ProjectTypeID;
            projectGet.Code = projectParaUpdate.Code;
            projectGet.Name = projectParaUpdate.Name;
            projectGet.Description = projectParaUpdate.Description;
            projectGet.StartDate = projectParaUpdate.StartDate;
            projectGet.EndDate = projectParaUpdate.EndDate;
            projectGet.ActualStartDate = projectParaUpdate.ActualStartDate;
            projectGet.ActualEndDate = projectParaUpdate.ActualEndDate;
            projectGet.Budget = projectParaUpdate.Budget;
            projectGet.Hours = projectParaUpdate.Hours;
            projectGet.WaitingStartDate = projectParaUpdate.WaitingStartDate;
            projectGet.ActualEndDate = projectParaUpdate.WaitingEndDate;
            projectGet.Observation = projectParaUpdate.Observation;

            if (projectGet.StartDate > projectGet.EndDate)
            {
                throw new ClientFaultException("No puede ingresar una fecha de Fin anterior a la fecha de Anterior.", 401);
            }

            var projectUpdated = await projectRepository.UpdateProjectAsync(projectGet);

            return _mapper.Map<UpdateProjectResponse>(projectUpdated);
        }

        public async Task<ActiveInactiveProjectResponse> InactiveProject(int projectId)
        {

            var projectInactive = await projectRepository.InactivateProjectAsync(projectId);

            return _mapper.Map<ActiveInactiveProjectResponse>(projectInactive);
        }

        public async Task<ActiveInactiveProjectResponse> ActiveProject(int ProjectId)
        {

            var projectActive = await projectRepository.ActivateProjectAsync(ProjectId);

            return _mapper.Map<ActiveInactiveProjectResponse>(projectActive);
        }

        public async Task AssignEmployeesToProject(AssignEmployeesToProjectRequest request)
        {
            foreach (var dto in request.EmployeeProjectMiddle)
            {
                bool tieneEmpleado = dto.EmployeeId.HasValue;
                bool tieneProveedor = dto.SupplierID.HasValue;

                if (tieneEmpleado == tieneProveedor)
                {
                    throw new ArgumentException(
                        $"Cada asignación debe tener solo EmployeeId o solo SupplierID. " +
                        $"DTO con EmployeeId={dto.EmployeeId} SupplierID={dto.SupplierID} no válido."
                    );
                }
            }

            var existing = await projectRepository.GetByProjectEmployeeIDAsync(request.ProjectID);
            var now = DateTime.UtcNow;
            var finalList = new List<EmployeeProject>();

            foreach (var dto in request.EmployeeProjectMiddle)
            {
                var match = existing.FirstOrDefault(ep =>
                    ep.EmployeeID == dto.EmployeeId &&
                    ep.SupplierID == dto.SupplierID
                );

                if (match == null)
                {
                    finalList.Add(new EmployeeProject
                    {
                        ProjectID = request.ProjectID,
                        EmployeeID = dto.EmployeeId,
                        SupplierID = dto.SupplierID,
                        AssignedRole = dto.AssignedRole,
                        CostPerHour = dto.CostPerHour,
                        AllocatedHours = dto.AllocatedHours,
                        Status = true,
                        AssignmentDate = now,
                        CreationDate = now,
                        CreationUser = "SYSTEM"
                    });
                }
                else
                {
                    if (!match.Status)
                    {
                        match.Status = true;
                        match.ModificationDate = now;
                        match.ModificationUser = "SYSTEM";
                    }

                    match.AssignedRole = dto.AssignedRole;
                    match.CostPerHour = dto.CostPerHour;
                    match.AllocatedHours = dto.AllocatedHours;

                    finalList.Add(match);
                }
            }

            foreach (var ep in existing)
            {
                bool sigueEnRequest = request.EmployeeProjectMiddle.Any(dto =>
                    dto.EmployeeId == ep.EmployeeID &&
                    dto.SupplierID == ep.SupplierID
                );

                if (!sigueEnRequest && ep.Status)
                {
                    ep.Status = false;
                    ep.ModificationDate = now;
                    ep.ModificationUser = "SYSTEM";
                    finalList.Add(ep);
                }
            }

            await projectRepository.SaveAssignmentsAsync(finalList);
        }

        public async Task<GetProjectDetailByIDResponse?> GetProjectDetailByID(int projectID)
        {
            var project = await projectRepository.GetProjectDetailByIDAsync(projectID);

            if (project == null)
                return null;

            var response = new GetProjectDetailByIDResponse
            {
                Id = project.Id,
                ClientID = project.ClientID,
                ProjectStatusID = project.ProjectStatusID,
                Code = project.Code,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ActualStartDate = project.ActualStartDate,
                ActualEndDate = project.ActualEndDate,
                Budget = project.Budget,
                WaitingEndDate = project.WaitingEndDate,
                WaitingStartDate = project.WaitingStartDate,
                Observation = project.Observation,

                EmployeeProjects = project.EmployeeProject.Select(ep => new GetEmployeeProjectResponse
                {
                    Id = ep.Id,
                    EmployeeID = ep.EmployeeID,
                    SupplierID = ep.SupplierID,
                    AssignedRole = ep.AssignedRole,
                    CostPerHour = ep.CostPerHour,
                    AllocatedHours = ep.AllocatedHours,
                    ProjectID = ep.ProjectID,
                    Status = ep.Status
                }).ToList(),

                EmployeesPersonInfo = project.EmployeeProject
                    .Where(ep => ep.Employee != null)
                    .Select(ep => ep.Employee)
                    .Distinct()
                    .Select(e => new GetEmployeesPersonInfoResponse
                    {
                        Id = e.Id,
                        PersonID = e.PersonID,
                        EmployeeCode = e.EmployeeCode,
                        IdentificationNumber = e.Person.IdentificationNumber,
                        FirstName = e.Person.FirstName,
                        LastName = e.Person.LastName,
                        Status = e.Status
                    }).ToList()
            };  

            return response;
        }

        public async Task<List<GetProjectsByEmployeeIDResponse>> GetProjectsByEmployeeIdAsync(int employeeId)
        {
            var projects = await projectRepository.GetProjectsByEmployeeIdAsync(employeeId);
            return _mapper.Map<List<GetProjectsByEmployeeIDResponse>>(projects);
        }
        public async Task<List<CreateDtoToExcelProject>> GetProjectsForExcelAsync()
        {
            // 1️ Traer todos los proyectos
            var projects = await projectRepository.GetAllProjectsAsync();
            if (projects == null || !projects.Any())
                return new List<CreateDtoToExcelProject>();

            var projectIds = projects.Select(p => p.Id).ToList();

            // 2️ Traer líderes activos por proyecto
            var leaders = await _leaderRepository.GetActiveLeadersByProjectIdsAsync(projectIds);

            // 3️ Traer clientes relacionados
            var clientIds = projects.Select(p => p.ClientID).Distinct().ToList();
            var clients = await _clientRepository.GetListOfClientsByIdsAsync(clientIds);

            // 4️ Mapear a DTO listo para Excel
            var result = projects.Select((p, index) =>
            {
                var projectLeaders = leaders
                    .Where(l => l.ProjectID == p.Id)
                    .Select(l => new LiderData
                    {
                        Id = l.Id,
                        GetPersonResponse = new GetPersonResponse
                        {
                            Id = l.Person.Id,
                            FirstName = l.Person.FirstName,
                            LastName = l.Person.LastName,
                            Email = l.Person.Email
                        }
                    })
                    .ToList();

                var projectClients = clients
                    .Where(c => c.Id == p.ClientID)
                    .Select(c => new ClientData
                    {
                        TradeName = c.TradeName

                    })
                    .ToList();

                return new CreateDtoToExcelProject
                {
                    Id = p.Id,
                    ClientID = p.ClientID,
                    ProjectType = new ProjectType { TypeName = p.ProjectType?.TypeName },
                    ProjectStatus = new ProjectStatus { StatusName = p.ProjectStatus?.StatusName },
                    Code = p.Code,
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    ActualStartDate = p.ActualStartDate,
                    ActualEndDate = p.ActualEndDate,
                    Budget = p.Budget,
                    Hours = p.Hours,
                    Status = p.Status,
                    WaitingStartDate = p.WaitingStartDate,
                    WaitingEndDate = p.WaitingEndDate,
                    Observation = p.Observation,
                    LiderData = projectLeaders,
                    ClientData = projectClients,

                };
            }).ToList();

            return result;
        }




        public async Task<byte[]> GenerateProjectsExcelAsync()
        {
            // Obtener los datos listos
            var projects = await GetProjectsForExcelAsync();

            if (projects == null || !projects.Any())
                return Array.Empty<byte>();

            using (var memoryStream = new MemoryStream())
            {
                using (var spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = spreadsheetDocument.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();
                    var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new SheetData();

                    worksheetPart.Worksheet = new Worksheet();
                    var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
                    var sheet = new Sheet
                    {
                        Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "Proyectos"
                    };
                    sheets.Append(sheet);

                    // Cabecera
                    var headerRow = new Row();
                    headerRow.Append(
                        CreateCell("NRO"),
                        CreateCell("CODIGO DEL PROYECTO"),
                        CreateCell("PROYECTO"),
                        CreateCell("LIDER"),
                        CreateCell("CLIENTE"),
                        CreateCell("ESTADO DEL PROYECTO"),
                        CreateCell("TIPO DE PROYECTO"),
                        CreateCell("FECHA DE INICIO"),
                        CreateCell("FECHA DE FIN ESTIMADA"),
                        CreateCell("FECHA FIN REAL"),
                        CreateCell("PRESUPUESTO"),
                        CreateCell("HORAS"),
                        CreateCell("FECHA INICIO ESPERA"),
                        CreateCell("FECHA FIN ESPERA"),
                        CreateCell("OBSERVACIONES")
                    );
                    sheetData.AppendChild(headerRow);

                    // Filas con los datos
                    int nro = 1;
                    foreach (var item in projects)
                    {
                        var row = new Row();
                        row.Append(
                            CreateCell(nro.ToString()),
                            CreateCell(item.Code),
                            CreateCell(item.Name),
                            CreateCell(item.LiderData?.FirstOrDefault() != null
                                        ? $"{item.LiderData.First().GetPersonResponse.FirstName} {item.LiderData.First().GetPersonResponse.LastName}"
                                        : string.Empty),
                            CreateCell(item.ClientData?.FirstOrDefault()?.TradeName ?? string.Empty),
                            CreateCell(item.ProjectStatus?.StatusName ?? ""),
                            CreateCell(item.ProjectType?.TypeName ?? ""),
                            CreateCell(item.StartDate?.ToShortDateString() ?? string.Empty),
                            CreateCell(item.EndDate?.ToShortDateString() ?? string.Empty),
                            CreateCell(item.ActualEndDate?.ToShortDateString() ?? string.Empty),
                            CreateCell(item.Budget?.ToString("N2") ?? "0"),
                            CreateCell(item.Hours.ToString()),
                            CreateCell(item.WaitingStartDate?.ToShortDateString() ?? string.Empty),
                            CreateCell(item.WaitingEndDate?.ToShortDateString() ?? string.Empty),
                            CreateCell(item.Observation ?? string.Empty)
                        );
                        sheetData.AppendChild(row);
                        nro++;
                    }

                    // Definir anchos de columnas dinámicos
                    var columns = new Columns();
                    columns.Append(CreateColumn(2, 2, CalculateColumnWidth(new[] { "Código Proyecto" }.Concat(projects.Select(p => p.Code)))));
                    columns.Append(CreateColumn(3, 3, CalculateColumnWidth(new[] { "Proyecto" }.Concat(projects.Select(p => p.Name)))));
                    columns.Append(CreateColumn(4, 4, CalculateColumnWidth(new[] { "Líder" }.Concat(projects.Select(p =>
                        p.LiderData?.FirstOrDefault() != null
                            ? $"{p.LiderData.First().GetPersonResponse.FirstName} {p.LiderData.First().GetPersonResponse.LastName}"
                            : string.Empty
                    )))));
                    columns.Append(CreateColumn(5, 5, CalculateColumnWidth(new[] { "Cliente" }.Concat(projects.Select(p => p.ClientData?.FirstOrDefault()?.TradeName ?? "")))));
                    columns.Append(CreateColumn(6, 6, CalculateColumnWidth(new[] { "Estado Proyecto" }.Concat(projects.Select(p => p.ProjectStatus?.StatusName ?? "")))));
                    columns.Append(CreateColumn(7, 7, CalculateColumnWidth(new[] { "Tipo Proyecto" }.Concat(projects.Select(p => p.ProjectType?.TypeName ?? "")))));
                    columns.Append(CreateColumn(8, 20, 22)); // Fechas
                    columns.Append(CreateColumn(11, 14, 18)); // Presupuesto
                    columns.Append(CreateColumn(12, 14, 15)); // Horas
                    columns.Append(CreateColumn(13, 25, 20)); // Fechas Espera
                    columns.Append(CreateColumn(15, 15, CalculateColumnWidth(new[] { "Observaciones" }.Concat(projects.Select(p => p.Observation ?? "")))));

                    worksheetPart.Worksheet.Append(columns);
                    worksheetPart.Worksheet.Append(sheetData);
                }

                return memoryStream.ToArray();
            }
        }

        // Helpers
        private Cell CreateCell(string value, uint styleIndex = 0)
        {
            return new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(value ?? ""),
                StyleIndex = styleIndex
            };
        }

        private Column CreateColumn(uint min, uint max, double width)
        {
            return new Column
            {
                Min = min,
                Max = max,
                Width = width,
                CustomWidth = true
            };
        }

        private double CalculateColumnWidth(IEnumerable<string> values)
        {
            if (!values.Any())
                return 10;

            int maxLength = values.Max(v => v?.Length ?? 0);

            // Aproximación: cada carácter ~1.2 unidades en Excel
            return Math.Min(100, maxLength * 1.2);
        }

    }

}

