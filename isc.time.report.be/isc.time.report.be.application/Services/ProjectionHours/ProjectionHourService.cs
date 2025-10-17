using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using isc.time.report.be.application.Interfaces.Repository.ProjectionHours;
using isc.time.report.be.application.Interfaces.Repository.Projections;
using isc.time.report.be.application.Interfaces.Service.ProjectionHours;
using isc.time.report.be.domain.Entity.ProjectionHours;
using isc.time.report.be.domain.Entity.Projections;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Request.Projections;
using isc.time.report.be.domain.Models.Response.Projections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Services.ProjectionHours
{
    public class ProjectionHourService : IProjectionHourService
    {
        private readonly IProjectionHoursRepository _projectionHoursRepository;
        private readonly IMapper _mapper;


        public ProjectionHourService(IProjectionHoursRepository projectionHoursRepository, IMapper mapper)
        {
            _projectionHoursRepository = projectionHoursRepository;
            _mapper = mapper;
        }

        public async Task<List<ProjectionWithoutProjectResponse>> GetAllProjectionByProjectId(Guid projectionId)
        {
            var result = await _projectionHoursRepository.GetAllProjectionsWithoutProjectAsync(projectionId);

            if (result.Any())
            {
                return result;
            }
            else
            {
                throw new ClientFaultException("No se encontraron recursos para la proyeccion especificada.");
            }
        }

        public async Task<CreateProjectionWithoutProjectResponse> CreateAsync(CreateProjectionWithoutProjectRequest request)
        {
            // Si el frontend no envía GroupProjection, generamos uno nuevo
            if (request.GroupProjection == null)
            {
                request.GroupProjection = Guid.NewGuid();
            }

            // Mapeo del request a la entidad
            var entity = _mapper.Map<ProjectionHour>(request);

            // Aseguramos que la entidad tenga el GroupProjection correcto
            entity.GroupProjection = request.GroupProjection;

            // Serializamos la distribución de tiempo
            entity.TimeDistribution = JsonSerializer.Serialize(request.TimeDistribution);

            // Guardamos en la base de datos
            await _projectionHoursRepository.CreateProjectionWithoutProjectAsync(entity);

            // Construimos la respuesta
            var response = new CreateProjectionWithoutProjectResponse
            {
                GroupProjection = entity.GroupProjection,
                ResourceTypeId = entity.ResourceTypeId,
                ResourceName = entity.ResourceName,
                ProjectionName = entity.ProjectionName,
                HourlyCost = entity.HourlyCost,
                ResourceQuantity = entity.ResourceQuantity,
                TotalTime = entity.TotalTime,
                ResourceCost = entity.ResourceCost,
                ParticipationPercentage = entity.ParticipationPercentage,
                PeriodType = entity.PeriodType,
                PeriodQuantity = entity.PeriodQuantity,
                TimeDistribution = string.IsNullOrEmpty(entity.TimeDistribution)
                    ? new List<double>()
                    : JsonSerializer.Deserialize<List<double>>(entity.TimeDistribution)
            };

            return response;
        }


        public async Task<UpdateProjectionWithoutProjectResponse> UpdateAsync(UpdateProjectionWithoutProjectRequest request, Guid groupProjection, int resourceTypeId)
        {
            // 1️⃣ Buscar el registro existente
            var entity = await _projectionHoursRepository.GetResourceByProjectionIdAsync(groupProjection, resourceTypeId);

            if (entity == null)
            {
                throw new Exception($"No se encontró el recurso con GroupProjection = {groupProjection} y ResourceTypeId = {resourceTypeId}");
            }

            // 2️⃣ Actualizar los campos
            entity.ResourceTypeId = request.ResourceTypeId;
            entity.ResourceName = request.ResourceName;
            entity.ProjectionName = request.ProjectionName ?? entity.ProjectionName;
            entity.HourlyCost = request.HourlyCost;
            entity.ResourceQuantity = request.ResourceQuantity;
            entity.TimeDistribution = JsonSerializer.Serialize(request.TimeDistribution);
            entity.TotalTime = request.TotalTime;
            entity.ResourceCost = request.ResourceCost;
            entity.ParticipationPercentage = request.ParticipationPercentage;

            entity.ModificationDate = DateTime.UtcNow;

            // 3️⃣ Guardar cambios
            await _projectionHoursRepository.UpdateResourceAssignedToProjectionAsync(entity);

            // 4️⃣ Construir y devolver respuesta
            var response = new UpdateProjectionWithoutProjectResponse
            {
                GroupProjection = entity.GroupProjection,
                ResourceTypeId = entity.ResourceTypeId,
                ResourceName = entity.ResourceName,
                ProjectionName = entity.ProjectionName,
                HourlyCost = entity.HourlyCost,
                ResourceQuantity = entity.ResourceQuantity,
                TimeDistribution = string.IsNullOrEmpty(entity.TimeDistribution)
                    ? new List<double>()
                    : JsonSerializer.Deserialize<List<double>>(entity.TimeDistribution),
                TotalTime = entity.TotalTime,
                ResourceCost = entity.ResourceCost,
                ParticipationPercentage = entity.ParticipationPercentage
            };

            return response;
        }



        public async Task ActivateInactiveResourceAsync(Guid groupProjection, int resourceTypeId, bool active)
        {
            var rowsAffected = await _projectionHoursRepository.ActiveInactiveResourceOfProjectionWithoutProjectAsync(groupProjection, resourceTypeId, active);

            if (rowsAffected == 0)
            {
                throw new ServerFaultException($"Recurso {resourceTypeId} no encontrado en el projecto {groupProjection}");
            }
        }

        public async Task<byte[]> ExportProjectionWithoutProjectToExcelAsync(Guid groupProjectionId)
        {
            // 🔹 Obtener todas las proyecciones asociadas al grupo
            var data = await GetAllProjectionByProjectId(groupProjectionId);

            if (data == null || !data.Any())
                throw new Exception("No hay proyecciones registradas para este grupo.");

            var first = data.First(); // usamos el primer registro para encabezados generales

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
                    Name = "Projection Without Project"
                });

                // -------------------------- TÍTULO PRINCIPAL --------------------------
                var titleRow = new Row();
                titleRow.Append(CreateCellModel("Distribución de Horas - Sin Proyecto", 4));
                sheetData.Append(titleRow);

                // -------------------------- MERGE CELLS PARA EL TÍTULO --------------------------
                var mergeCells = worksheetPart.Worksheet.Elements<MergeCells>().FirstOrDefault();
                if (mergeCells == null)
                {
                    mergeCells = new MergeCells();
                    worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.Elements<SheetData>().First());
                }

                mergeCells.Append(new MergeCell { Reference = new StringValue("A1:K1") });

                // -------------------------- CABECERA --------------------------
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
                    CreateCellModel("Porcentaje de Participación", 1)
                );

                sheetData.Append(headerRow);

                // -------------------------- TOTALES --------------------------
                int totalResourceQuantity = 0;
                List<double> totalDistribution = Enumerable.Repeat(0.0, periodQty).ToList();
                decimal totalTime = 0m;
                decimal totalResourceCost = 0m;
                decimal totalParticipation = 0m;

                // -------------------------- FILAS DE DATOS --------------------------
                foreach (var item in data)
                {
                    var row = new Row();
                    row.Append(

                        CreateCellModel(item.resource_name, 2),
                        CreateCellModel(item.hourly_cost.ToString("F2"), 2),
                        CreateCellModel(item.resource_quantity.ToString(), 2)
                    );

                    var distribution = JsonSerializer.Deserialize<List<double>>(item.time_distribution) ?? new List<double>();

                    for (int i = 0; i < periodQty; i++)
                    {
                        double value = (i < distribution.Count) ? distribution[i] : 0;
                        row.Append(CreateCellModel(value.ToString(), 2));
                        totalDistribution[i] += value;
                    }

                    row.Append(
                        CreateCellModel(item.total_time.ToString("F2"), 2),
                        CreateCellModel(item.resource_cost.ToString("F2"), 2)
                    );

                    decimal valorExcel = item.participation_percentage;
                    row.Append(CreateNumberCell(valorExcel / 100, 3));

                    sheetData.Append(row);

                    // 🔹 Acumular totales
                    totalResourceQuantity += item.resource_quantity;
                    totalTime += item.total_time;
                    totalResourceCost += item.resource_cost;
                    totalParticipation += item.participation_percentage;

                    if (totalParticipation > 99.90m && totalParticipation < 100.01m)
                        totalParticipation = 100.00m;
                    else if (totalParticipation > 100.00m)
                        totalParticipation = 100.00m;
                }

                // -------------------------- TOTAL GENERAL --------------------------
                var totalRow = new Row();
                totalRow.Append(
                    CreateCellModel("Total", 1),
                    CreateCellModel("", 1),
                    CreateCellModel("", 1),
                    CreateCellModel(totalResourceQuantity.ToString(), 1)
                );

                for (int i = 0; i < periodQty; i++)
                    totalRow.Append(CreateCellModel(totalDistribution[i].ToString(), 1));

                totalRow.Append(
                    CreateCellModel(totalTime.ToString("F2"), 1),
                    CreateCellModel(totalResourceCost.ToString("F2"), 1),
                    CreateNumberCell(totalParticipation / 100, 3)
                );

                sheetData.Append(totalRow);

                // 🔹 Merge para el total
                uint totalRowIndex = (uint)sheetData.ChildElements.Count;
                mergeCells.Append(new MergeCell()
                {
                    Reference = new StringValue($"A{totalRowIndex}:C{totalRowIndex}")
                });
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
                    ),
                    new Font( // 2 - Fuente para título principal
                        new FontSize { Val = 16 }, // tamaño más grande
                        new Bold(),                // negrita
                        new FontName { Val = "Calibri" }
)
                ),

                new Fills(
                    new Fill(new PatternFill { PatternType = PatternValues.None }), // 0 - Default
                    new Fill(new PatternFill { PatternType = PatternValues.Gray125 }), // 1 - Default
                    new Fill(
                        new PatternFill(
                            new ForegroundColor { Rgb = "F2F2F2" } // color cabecera
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
                        FontId = 1,
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
                        ApplyFill = true,
                        ApplyBorder = true,
                        ApplyAlignment = true
                    },
                    new CellFormat // 4 - Título principal (más grande y negrita)
                    {
                        FontId = 2, // Debemos definir una nueva fuente con tamaño mayor y bold
                        FillId = 0, // sin fondo o puedes ponerle uno si quieres
                        BorderId = 0, // sin bordes
                        Alignment = new Alignment
                        {
                            Horizontal = HorizontalAlignmentValues.Center,
                            Vertical = VerticalAlignmentValues.Center,
                            WrapText = true
                        },
                        ApplyFont = true,
                        ApplyFill = false,
                        ApplyBorder = false,
                        ApplyAlignment = true
                    }


                )
            );
        }
    
}
}
