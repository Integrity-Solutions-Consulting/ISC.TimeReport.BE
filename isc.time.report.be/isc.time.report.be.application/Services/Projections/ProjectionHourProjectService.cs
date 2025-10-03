using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Chart = DocumentFormat.OpenXml.Drawing.Chart;
using Formula = DocumentFormat.OpenXml.Drawing.Charts.Formula;
using NumberingFormat = DocumentFormat.OpenXml.Drawing.Charts.NumberingFormat;
using OrientationValues = DocumentFormat.OpenXml.Drawing.Charts.OrientationValues;
using Values = DocumentFormat.OpenXml.Drawing.Charts.Values;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;


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
            var data = await GetAllProjectionByProjectId(projectId);
            if (data == null || !data.Any())
                throw new Exception("No hay proyección para este proyecto.");

            using var memoryStream = new MemoryStream();

            using (var spreadsheet = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                // --------------------------
                // Workbook y Worksheet
                // --------------------------
                var workbookPart = spreadsheet.AddWorkbookPart();
                workbookPart.Workbook = new Workbook
                {
                    CalculationProperties = new CalculationProperties
                    {
                        FullCalculationOnLoad = true,
                        CalculationId = 125725
                    }
                };

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                var sheets = spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());
                sheets.Append(new Sheet
                {
                    Id = spreadsheet.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Proyección"
                });

                // --------------------------
                // Cabecera
                // --------------------------
                int maxPeriods = data.Max(d => d.period_quantity);
                bool isMonth = data.First().period_type;

                var headerRow = new Row();
                headerRow.Append(
                    CreateTextCell("Tipo de Recurso"),
                    CreateTextCell("Nombre Recurso"),
                    CreateTextCell("Costo por Hora / Tipo Recurso"),
                    CreateTextCell("Cantidad de Recursos")
                );

                for (int i = 1; i <= maxPeriods; i++)
                    headerRow.Append(CreateTextCell(isMonth ? $"Mes {i}" : $"Semana {i}"));

                headerRow.Append(
                    CreateTextCell("Tiempo Total"),
                    CreateTextCell("Costo Recursos"),
                    CreateTextCell("Porcentaje de Participación")
                );

                sheetData.Append(headerRow);

                // --------------------------
                // Datos
                // --------------------------
                int rowIndex = 2;
                foreach (var item in data)
                {
                    var row = new Row();
                    row.Append(
                        CreateTextCell(item.ResourceTypeName),
                        CreateTextCell(item.resource_name),
                        CreateNumberCell(item.hourly_cost.ToString("F2")),
                        CreateNumberCell(item.resource_quantity.ToString())
                    );

                    var distribution = JsonSerializer.Deserialize<List<int>>(item.time_distribution);
                    for (int i = 0; i < item.period_quantity; i++)
                        row.Append(CreateNumberCell(distribution != null && i < distribution.Count ? distribution[i].ToString() : "0"));

                    for (int i = item.period_quantity; i < maxPeriods; i++)
                        row.Append(CreateTextCell(""));

                    row.Append(
                        CreateNumberCell(item.total_time.ToString("F2")),
                        CreateNumberCell(item.resource_cost.ToString("F2")),
                        CreateNumberCell(item.participation_percentage.ToString("P2"))
                    );

                    sheetData.Append(row);
                    rowIndex++;
                }

                workbookPart.Workbook.Save();

                // --------------------------
                // Insertar gráfico si hay datos
                // --------------------------
                if (maxPeriods > 0)
                {
                    InsertLineChart(
                        worksheetPart,
                        startRow: 2,
                        startColumn: 5,
                        endRow: rowIndex - 1,
                        endColumn: 5 + maxPeriods - 1,
                        chartStartRow: 1,
                        chartStartColumn: 5,
                        chartTitle: "Proyección de Recursos"
                    );
                }

                workbookPart.Workbook.Save();
            }

            return memoryStream.ToArray();
        }

        private void InsertLineChart(
            DocumentFormat.OpenXml.Packaging.WorksheetPart worksheetPart,
            int startRow,
            int startColumn,
            int endRow,
            int endColumn,
            int chartStartRow,
            int chartStartColumn,
            string chartTitle)
        {
            // --------------------------
            // Crear DrawingsPart si no existe
            // --------------------------
            var drawingsPart = worksheetPart.DrawingsPart;
            if (drawingsPart == null)
            {
                drawingsPart = worksheetPart.AddNewPart<DocumentFormat.OpenXml.Packaging.DrawingsPart>();
                drawingsPart.WorksheetDrawing = new DocumentFormat.OpenXml.Drawing.Spreadsheet.WorksheetDrawing();
                worksheetPart.Worksheet.Append(new DocumentFormat.OpenXml.Spreadsheet.Drawing
                {
                    Id = worksheetPart.GetIdOfPart(drawingsPart)
                });
            }

            // --------------------------
            // Crear ChartPart
            // --------------------------
            var chartPart = drawingsPart.AddNewPart<DocumentFormat.OpenXml.Packaging.ChartPart>();
            chartPart.ChartSpace = new DocumentFormat.OpenXml.Drawing.Charts.ChartSpace();
            chartPart.ChartSpace.Append(new DocumentFormat.OpenXml.Drawing.Charts.EditingLanguage { Val = "en-US" });

            // --------------------------
            // Crear gráfico de líneas
            // --------------------------
            var chart = chartPart.ChartSpace.AppendChild(new DocumentFormat.OpenXml.Drawing.Charts.Chart());
            var plotArea = chart.AppendChild(new DocumentFormat.OpenXml.Drawing.Charts.PlotArea());
            plotArea.AppendChild(new DocumentFormat.OpenXml.Drawing.Charts.Layout());

            var lineChart = plotArea.AppendChild(new DocumentFormat.OpenXml.Drawing.Charts.LineChart());
            lineChart.AppendChild(new DocumentFormat.OpenXml.Drawing.Charts.VaryColors() { Val = false });

            var sheetData = worksheetPart.Worksheet.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.SheetData>();

            // Cada fila = serie
            for (int row = startRow; row <= endRow; row++)
            {
                var series = new DocumentFormat.OpenXml.Drawing.Charts.LineChartSeries(
                    new DocumentFormat.OpenXml.Drawing.Charts.Index() { Val = (uint)(row - startRow) },
                    new DocumentFormat.OpenXml.Drawing.Charts.Order() { Val = (uint)(row - startRow) },
                    new DocumentFormat.OpenXml.Drawing.Charts.SeriesText(
                        new DocumentFormat.OpenXml.Drawing.Charts.StringReference(
                            new DocumentFormat.OpenXml.Drawing.Charts.Formula($"A{row}")
                        )
                    )
                );

                var cat = new DocumentFormat.OpenXml.Drawing.Charts.CategoryAxisData(
                    new DocumentFormat.OpenXml.Drawing.Charts.StringReference(
                        new DocumentFormat.OpenXml.Drawing.Charts.Formula($"$B${startRow}:${ConvertToColumnLetter(endColumn)}${startRow}")
                    )
                );

                var val = new DocumentFormat.OpenXml.Drawing.Charts.Values(
                    new DocumentFormat.OpenXml.Drawing.Charts.NumberReference(
                        new DocumentFormat.OpenXml.Drawing.Charts.Formula($"$B${row}:${ConvertToColumnLetter(endColumn)}${row}")
                    )
                );

                series.Append(cat);
                series.Append(val);
                lineChart.Append(series);
            }

            // --------------------------
            // Ejes
            // --------------------------
            uint catAxId = 48650112u;
            uint valAxId = 48672768u;

            plotArea.AppendChild(new DocumentFormat.OpenXml.Drawing.Charts.CategoryAxis(
                new DocumentFormat.OpenXml.Drawing.Charts.AxisId() { Val = catAxId },
                new DocumentFormat.OpenXml.Drawing.Charts.Scaling(new DocumentFormat.OpenXml.Drawing.Charts.Orientation() { Val = DocumentFormat.OpenXml.Drawing.Charts.OrientationValues.MinMax }),
                new DocumentFormat.OpenXml.Drawing.Charts.AxisPosition() { Val = DocumentFormat.OpenXml.Drawing.Charts.AxisPositionValues.Bottom },
                new DocumentFormat.OpenXml.Drawing.Charts.TickLabelPosition() { Val = DocumentFormat.OpenXml.Drawing.Charts.TickLabelPositionValues.NextTo },
                new DocumentFormat.OpenXml.Drawing.Charts.CrossingAxis() { Val = valAxId },
                new DocumentFormat.OpenXml.Drawing.Charts.Crosses() { Val = DocumentFormat.OpenXml.Drawing.Charts.CrossesValues.AutoZero }
            ));

            plotArea.AppendChild(new DocumentFormat.OpenXml.Drawing.Charts.ValueAxis(
                new DocumentFormat.OpenXml.Drawing.Charts.AxisId() { Val = valAxId },
                new DocumentFormat.OpenXml.Drawing.Charts.Scaling(new DocumentFormat.OpenXml.Drawing.Charts.Orientation() { Val = DocumentFormat.OpenXml.Drawing.Charts.OrientationValues.MinMax }),
                new DocumentFormat.OpenXml.Drawing.Charts.AxisPosition() { Val = DocumentFormat.OpenXml.Drawing.Charts.AxisPositionValues.Left },
                new DocumentFormat.OpenXml.Drawing.Charts.MajorGridlines(),
                new DocumentFormat.OpenXml.Drawing.Charts.NumberingFormat() { FormatCode = "General", SourceLinked = true },
                new DocumentFormat.OpenXml.Drawing.Charts.TickLabelPosition() { Val = DocumentFormat.OpenXml.Drawing.Charts.TickLabelPositionValues.NextTo },
                new DocumentFormat.OpenXml.Drawing.Charts.CrossingAxis() { Val = catAxId },
                new DocumentFormat.OpenXml.Drawing.Charts.Crosses() { Val = DocumentFormat.OpenXml.Drawing.Charts.CrossesValues.AutoZero }
            ));

            // --------------------------
            // Título
            // --------------------------
            chart.Append(new DocumentFormat.OpenXml.Drawing.Charts.Title(
                new DocumentFormat.OpenXml.Drawing.Charts.ChartText(
                    new DocumentFormat.OpenXml.Drawing.Charts.RichText(
                        new DocumentFormat.OpenXml.Drawing.Text() { Text = chartTitle }
                    )
                )
            ));

            chartPart.ChartSpace.Save();

            // --------------------------
            // Posicionar gráfico en la hoja
            // --------------------------
            var fromMarker = new DocumentFormat.OpenXml.Drawing.Spreadsheet.FromMarker();
            fromMarker.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.ColumnId((chartStartColumn - 1).ToString()));
            fromMarker.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.ColumnOffset("0"));
            fromMarker.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.RowId((chartStartRow - 1).ToString()));
            fromMarker.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.RowOffset("0"));

            var toMarker = new DocumentFormat.OpenXml.Drawing.Spreadsheet.ToMarker();
            toMarker.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.ColumnId((chartStartColumn + 7).ToString()));
            toMarker.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.ColumnOffset("0"));
            toMarker.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.RowId((chartStartRow + 15).ToString()));
            toMarker.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.RowOffset("0"));

            var graphicFrame = new DocumentFormat.OpenXml.Drawing.Spreadsheet.GraphicFrame
            {
                NonVisualGraphicFrameProperties = new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualGraphicFrameProperties(
                    new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties { Id = 2u, Name = "Chart 1" },
                    new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualGraphicFrameDrawingProperties()
                ),
                Graphic = new DocumentFormat.OpenXml.Drawing.Graphic(
                    new DocumentFormat.OpenXml.Drawing.GraphicData(
                        new DocumentFormat.OpenXml.Drawing.Charts.ChartReference { Id = drawingsPart.GetIdOfPart(chartPart) }
                    )
                    { Uri = "http://schemas.openxmlformats.org/drawingml/2006/chart" }
                )
            };

            var twoCellAnchor = new DocumentFormat.OpenXml.Drawing.Spreadsheet.TwoCellAnchor();
            twoCellAnchor.Append(fromMarker);
            twoCellAnchor.Append(toMarker);
            twoCellAnchor.Append(graphicFrame);
            twoCellAnchor.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.ClientData());

            drawingsPart.WorksheetDrawing.Append(twoCellAnchor);
            drawingsPart.WorksheetDrawing.Save();
            worksheetPart.Worksheet.Save();
        }

        // --------------------------
        // Métodos auxiliares
        // --------------------------
        private string ConvertToColumnLetter(int column)
        {
            string columnString = "";
            while (column > 0)
            {
                int modulo = (column - 1) % 26;
                columnString = Convert.ToChar(65 + modulo) + columnString;
                column = (column - modulo) / 26;
            }
            return columnString;
        }

        private Cell CreateTextCell(string value) =>
            new Cell { DataType = CellValues.String, CellValue = new CellValue(value ?? string.Empty) };

        private Cell CreateNumberCell(string value) =>
            new Cell { DataType = CellValues.Number, CellValue = new CellValue(value ?? "0") };





    }
}

