using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using isc.time.report.be.application.Interfaces.Repository.Projections;
using isc.time.report.be.application.Interfaces.Service.Projections;
using isc.time.report.be.domain.Entity.Projections;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Projections;
using isc.time.report.be.domain.Models.Response.Projections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace isc.time.report.be.application.Services.Projections
{
    public class ProjectionHourProjectService : IProjectionHourProjectService
    {
        private readonly IProjectionHourProjectRepository _projectionHourProjectRepository;
        private readonly IMapper _mapper;

        public ProjectionHourProjectService(IProjectionHourProjectRepository projectionHourProjectRepository, IMapper mapper)
        {

            _projectionHourProjectRepository = projectionHourProjectRepository;
            _mapper = mapper;

        }

        public async Task<List<ProjectionHoursProjectResponse>> GetAllProjectionByProjectId(int projectId)
        {
            var result = await _projectionHourProjectRepository.GetAllProjectionsAsync(projectId);

            if (result.Any())
            {
                return result;
            }
            else
            {
                throw new ClientFaultException("No se encontraron recursos para la proyeccion especificada.");
            }
        }


        public async Task<CreateProjectionHoursProjectResponse> CreateAsync(ProjectionHoursProjectRequest request, int projectId)
        {

            var entity = _mapper.Map<ProjectionHourProject>(request);

            entity.TimeDistribution = JsonSerializer.Serialize(request.TimeDistribution);

            await _projectionHourProjectRepository.CreateProjectionAsync(entity);


            var response = new CreateProjectionHoursProjectResponse
            {
                ResourceTypeId = entity.ResourceTypeId,
                ResourceName = entity.ResourceName,
                HourlyCost = entity.HourlyCost,
                ResourceQuantity = entity.ResourceQuantity,
                TotalTime = entity.TotalTime,
                ResourceCost = entity.ResourceCost,
                ParticipationPercentage = entity.ParticipationPercentage,
                PeriodType = entity.PeriodType,
                PeriodQuantity = entity.PeriodQuantity,
                ProjecId = entity.ProjectId,
                TimeDistribution = string.IsNullOrEmpty(entity.TimeDistribution)
                    ? new List<int>()
                    : JsonSerializer.Deserialize<List<int>>(entity.TimeDistribution)


            };

            return response;
        }

        public async Task<UpdateProjectionHoursProjectResponse> UpdateAsync(UpdateProjectionHoursProjectRequest request, int resourceTypeId, int projectId)
        {

            var entity = await _projectionHourProjectRepository.GetResourceByProjectionIdAsync(projectId, resourceTypeId);

            if (entity == null)
                throw new ClientFaultException("Registro no encontrado", 401);

            // Mapeo 
            entity.ResourceTypeId = request.ResourceTypeId;
            entity.ProjectId = projectId;
            entity.ResourceName = request.ResourceName;
            entity.HourlyCost = request.HourlyCost;
            entity.ResourceQuantity = request.ResourceQuantity;
            entity.TotalTime = request.TotalTime;
            entity.ResourceCost = request.ResourceCost;
            entity.ParticipationPercentage = request.ParticipationPercentage;


            // Serialización 
            entity.TimeDistribution = JsonSerializer.Serialize(request.TimeDistribution);

            //Guardamos
            await _projectionHourProjectRepository.UpdateResourceAssignedToProjectionAsync(entity, resourceTypeId, projectId);

            //Mapeo actualizado
            var response = new UpdateProjectionHoursProjectResponse
            {
                ResourceTypeId = entity.ResourceTypeId,
                ProjectId = entity.ProjectId,
                ResourceName = entity.ResourceName,
                HourlyCost = entity.HourlyCost,
                ResourceQuantity = entity.ResourceQuantity,
                TimeDistribution = string.IsNullOrEmpty(entity.TimeDistribution)
                    ? new List<int>()
                    : JsonSerializer.Deserialize<List<int>>(entity.TimeDistribution),
                TotalTime = entity.TotalTime,
                ResourceCost = entity.ResourceCost,
                ParticipationPercentage = entity.ParticipationPercentage,

            };

            return response;
        }

        public async Task ActivateInactiveResourceAsync(int projectId, int resourceTypeId, bool active)
        {
            var rowsAffected = await _projectionHourProjectRepository.ActiveInactiveResourceOfProjectionAsync(projectId, resourceTypeId, active);

            if (rowsAffected == 0)
            {
                throw new ServerFaultException($"Recurso {resourceTypeId} no encontrado en el projecto {projectId}");
            }
        }


        public async Task<byte[]> ExportProjectionToExcelAsync(int projectId)
        {
            // Trae la lista completa del SP
            var data = await GetAllProjectionByProjectId(projectId);

            if (data == null || !data.Any())
                throw new Exception("No hay proyección para este proyecto.");

            var first = data.First(); // Aquí tomamos los campos generales

            using var memoryStream = new MemoryStream();

            using (var spreadsheet = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = spreadsheet.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylesPart.Stylesheet = CreateStylesheetModel();
                stylesPart.Stylesheet.Save();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                var sheetData = new SheetData();
                worksheetPart.Worksheet.Append(sheetData);

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                sheets.Append(new Sheet
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Projection Report"
                });

                // --------------------------
                // CABECERA DINÁMICA
                // --------------------------
                bool isWeekly = first.period_type;
                int periodQty = first.period_quantity;

                var headerRow = new Row();
                headerRow.Append(
                    CreateCellModel("Tipo de Recurso", 1),
                    CreateCellModel("Nombre del Recurso", 1),
                    CreateCellModel("Costo por Hora", 1),
                    CreateCellModel("Cantidad de Recursos", 1)
                );

                for (int i = 1; i <= periodQty; i++)
                {
                    string headerName = isWeekly ? $"Semana {i}" : $"Mes {i}";
                    headerRow.Append(CreateCellModel(headerName, 1));
                }

                headerRow.Append(
                    CreateCellModel("Tiempo Total", 1),
                    CreateCellModel("Costo Recursos", 1),
                    CreateCellModel("Porcentaje de Participacion", 1)
                );

                sheetData.Append(headerRow);

                // --------------------------
                // FILAS DE DATOS
                // --------------------------
                foreach (var item in data)
                {
                    var row = new Row();
                    row.Append(
                        CreateCellModel(item.ResourceTypeName, 2),
                        CreateCellModel(item.resource_name, 2),
                        CreateCellModel(item.hourly_cost.ToString("F2"), 2),
                        CreateCellModel(item.resource_quantity.ToString(), 2)
                    );

                    var distribution = JsonSerializer.Deserialize<List<int>>(item.time_distribution);

                    for (int i = 0; i < periodQty; i++)
                        row.Append(CreateCellModel(distribution != null && i < distribution.Count ? distribution[i].ToString() : "0", 2));

                    row.Append(
                        CreateCellModel(item.total_time.ToString("F2"), 2),
                        CreateCellModel(item.resource_cost.ToString("F2"), 2)
                    );
                    decimal valorExcel = item.participation_percentage;
                    row.Append(CreateNumberCell(valorExcel/100, 3));


                    sheetData.Append(row);
                }
            }

            return memoryStream.ToArray();
        }



        // Helpers
        private Cell CreateCellModel(string value, uint styleIndex = 0)
        {
            return new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(value ?? ""),
                StyleIndex = styleIndex
            };
        }
        private Cell CreateNumberCell(decimal value, uint styleIndex = 0)
        {
            return new Cell
            {
                DataType = CellValues.Number, // aquí no es string
                CellValue = new CellValue(value.ToString(CultureInfo.InvariantCulture)),
                StyleIndex = styleIndex
            };
        }


        private Column CreateColumnModel(uint min, uint max, double width)
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

        private Stylesheet CreateStylesheetModel()
        {
            return new Stylesheet(
                new Fonts(
                    new Font( // 0 - Default Calibri 11
                        new FontSize { Val = 10 },
                        new FontName { Val = "Calibri" }
                    ),
                    new Font( // 1 - Bold Calibri 11 (cabeceras)
                        new FontSize { Val = 10 },
                        new Bold(),
                        new FontName { Val = "Calibri" }
                    )
                ),

                new Fills(
                    new Fill(new PatternFill { PatternType = PatternValues.None }), // 0 - Default
                    new Fill(new PatternFill { PatternType = PatternValues.Gray125 }), // 1 - Default
                    new Fill(
                        new PatternFill(
                            new ForegroundColor { Rgb = "FFB0B0B0" } // color cabecera
                        )
                        { PatternType = PatternValues.Solid }
                    )
                ),

                new Borders(
                    new Border(), // 0 - Sin bordes
                    new Border(   // 1 - Bordes completos
                        new LeftBorder { Style = BorderStyleValues.Thin },
                        new RightBorder { Style = BorderStyleValues.Thin },
                        new TopBorder { Style = BorderStyleValues.Thin },
                        new BottomBorder { Style = BorderStyleValues.Thin },
                        new DiagonalBorder()
                    )
                ),

                new CellFormats(
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 0 }, // 0 - Default
                    new CellFormat // 1 - Cabeceras
                    {
                        FontId = 1,
                        FillId = 2, // color cabecera
                        BorderId = 1,
                        Alignment = new Alignment
                        {
                            Horizontal = HorizontalAlignmentValues.Center,
                            Vertical = VerticalAlignmentValues.Center,
                            WrapText = true
                        },
                        ApplyFont = true,
                        ApplyFill = true,
                        ApplyBorder = true,
                        ApplyAlignment = true
                    },
                    new CellFormat // 2 - Celdas normales con bordes
                    {
                        FontId = 0,
                        FillId = 0,
                        BorderId = 1,
                        Alignment = new Alignment
                        {
                            Horizontal = HorizontalAlignmentValues.Left,
                            Vertical = VerticalAlignmentValues.Center,
                            WrapText = true
                        },
                        ApplyFont = true,
                        ApplyFill = false,
                        ApplyBorder = true,
                        ApplyAlignment = true
                    },
                    new CellFormat // 3 - Porcentaje
                    {
                        FontId = 0,
                        FillId = 0,
                        BorderId = 1,
                        NumberFormatId = 10, // formato porcentaje con 2 decimales en Excel
                        Alignment = new Alignment
                        {
                            Horizontal = HorizontalAlignmentValues.Right,
                            Vertical = VerticalAlignmentValues.Center
                        },
                        ApplyNumberFormat = true,
                        ApplyFont = true,
                        ApplyFill = false,
                        ApplyBorder = true,
                        ApplyAlignment = true
                    }

                )
            );
        }
    }
}









