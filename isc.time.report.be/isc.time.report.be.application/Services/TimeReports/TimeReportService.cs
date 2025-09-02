using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using isc.time.report.be.application.Interfaces.Repository.Clients;
using isc.time.report.be.application.Interfaces.Repository.Employees;
using isc.time.report.be.application.Interfaces.Repository.Leaders;
using isc.time.report.be.application.Interfaces.Repository.Permissions;
using isc.time.report.be.application.Interfaces.Repository.Projects;
using isc.time.report.be.application.Interfaces.Repository.TimeReports;
using isc.time.report.be.application.Interfaces.Service.TimeReports;
using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Models.Dto.TimeReports;
using isc.time.report.be.domain.Models.Response.Dashboards;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = DocumentFormat.OpenXml.Drawing;
using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;

namespace isc.time.report.be.application.Services.TimeReports
{
    public class TimeReportService : ITimeReportService
    {
        private readonly IClientRepository clientRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly ITimeReportRepository timeReportRepository;
        private readonly ILeaderRepository leaderRepository;
        private readonly IPermissionRepository permissionRepository;
        private readonly IProjectRepository projectRepository;
        public TimeReportService(IClientRepository clientRepository, IEmployeeRepository employeeRepository, ITimeReportRepository timeReportRepository, ILeaderRepository leaderRepository, IPermissionRepository permissionRepository, IProjectRepository projectRepository)
        {
            this.clientRepository = clientRepository;
            this.employeeRepository = employeeRepository;
            this.timeReportRepository = timeReportRepository;
            this.leaderRepository = leaderRepository;
            this.permissionRepository = permissionRepository;
            this.projectRepository = projectRepository;
        }

        public async Task<byte[]> GenerateExcelReportAsync(int employeeId, int clientId, int year, int month, bool fullMonth)
        {
            
            // Obtener datos reales
            var reportData = await GetTimeReportDataFillAsync(employeeId, clientId, year, month, fullMonth);

            if (reportData.Activities == null || reportData.Activities.Count == 0)
            {
                throw new InvalidOperationException("No se puede generar el reporte: no hay actividades registradas.");
            }
            var holidays = await timeReportRepository.GetActiveHolidaysByMonthAndYearAsync(month, year);

            var permissions = await permissionRepository.GetPermissionsAprovedByEmployeeIdAsync(employeeId);

            var diasPermiso = new HashSet<DateOnly>();

            var permissionRanges = permissions
                .Select(p => new {
                    Start = DateOnly.FromDateTime(p.StartDate),
                    End = DateOnly.FromDateTime(p.EndDate)
                })
                .ToList();

            var primerLiderNombre = reportData.Activities.FirstOrDefault()?.LeaderName;

            using var stream = new MemoryStream();

            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.MacroEnabledWorkbook))
            {
                // Crear partes básicas del libro
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Agregar estilos
                var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylesPart.Stylesheet = CreateStylesheet();
                stylesPart.Stylesheet.Save();

                // Crear hoja de trabajo y contenido
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var worksheet = new Worksheet();
                var sheetData = new SheetData();

                // Definir anchos de columnas
                var columns = new Columns();
                columns.Append(CreateColumn(1, 1, 3.56));      // A
                columns.Append(CreateColumn(2, 2, 22));     // B
                columns.Append(CreateColumn(3, 3, 18.11));     // C
                columns.Append(CreateColumn(4, 4, 21.22));     // D
                columns.Append(CreateColumn(5, 5, 63.44));     // E
                columns.Append(CreateColumn(6, 6, 9.22));      // F
                for (uint i = 7; i <= 37; i++)                 // G to AK
                    columns.Append(CreateColumn(i, i, 3.56));

                columns.Append(CreateColumn(38, 38, 10.11));   // AL

                worksheet.Append(columns);
                worksheet.Append(sheetData);

                // Celdas combinadas
                var mergeCells = new MergeCells(
                    new MergeCell { Reference = "A1:AM1" },
                    new MergeCell { Reference = "F2:H2" },
                    new MergeCell { Reference = "I2:J2" },
                    new MergeCell { Reference = "A4:B4" },
                    new MergeCell { Reference = "C4:D4" },
                    new MergeCell { Reference = "A5:B5" },
                    new MergeCell { Reference = "C5:D5" },


                    new MergeCell { Reference = "A6:A8" },
                    new MergeCell { Reference = "B6:B8" },
                    new MergeCell { Reference = "C6:C8" },
                    new MergeCell { Reference = "D6:D8" },
                    new MergeCell { Reference = "E6:E8" },
                    new MergeCell { Reference = "F6:F8" },
                    new MergeCell { Reference = "G6:AK6" },
                    new MergeCell { Reference = "AL6:AL8" }
                );
                worksheet.Append(mergeCells);

                worksheetPart.Worksheet = worksheet;

                // Crear hoja
                    var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    sheets.Append(new Sheet
                    {
                        Id = workbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "TimeReport"
                    });

                // === Crear filas ===

                // Fila 1 - Título
                var row1 = new Row { RowIndex = 1, Height = 21, CustomHeight = true };
                row1.Append(CreateCell("TIME REPORT", 1));
                sheetData.Append(row1);

                // Fila 2 - Mes y Año
                var row2 = new Row { RowIndex = 2, Height = 18, CustomHeight = true };
                row2.Append(
                    CreateCell("", 0), CreateCell("", 0), CreateCell("", 0),
                    CreateCell("", 0), CreateCell("", 0),CreateCell(GetMonthName(month), 2),
                    CreateCell("", 0), CreateCell("", 0),
                    CreateCell(year.ToString(), 2)
                );
                sheetData.Append(row2);

                // Fila 3 - Vacía
                var row3 = new Row { RowIndex = 3, Height = 18, CustomHeight = true };
                sheetData.Append(row3);

                // Fila 4 - Cliente
                var row4 = new Row { RowIndex = 4, Height = 18, CustomHeight = true };
                row4.Append(
                    CreateCell("Cliente:", 3),
                    CreateCell("", 0),
                    CreateCell(reportData.TradeName, 3)
                );
                sheetData.Append(row4);

                // Fila 5 - Consultor
                var row5 = new Row { RowIndex = 5, Height = 16.8, CustomHeight = true };
                row5.Append(
                    CreateCell("Nombre del consultor:", 3),
                    CreateCell("", 0),
                    CreateCell(reportData.FirstName+" "+reportData.LastName, 3)


                    //aqui va lo de dias habiles

                );
                sheetData.Append(row5);

                // Fila 6 - Encabezados principales
                var row6 = new Row { RowIndex = 6, Height = 15, CustomHeight = true };

                // Añadir celdas fijas A–G
                row6.Append(
                    CreateCell("N°", 5),                                   // A
                    CreateCell("TIPO DE ACTIVIDAD", 4),                   // B
                    CreateCell("LIDER DE PROYECTO", 4),                   // C
                    CreateCell("CODIGO REQUERIMIENTO / INCIDENTE", 4),    // D
                    CreateCell("DESCRIPCION DE TRABAJOS REALIZADOS", 4),  // E
                    CreateCell("TOTAL HORAS POR ACTIVIDAD", 4),           // F
                    CreateCell("DISTRIBUCION DE TIEMPO DEL DIA", 4)       // G
                );

                // Añadir celdas G–AK: una por cada día del mes (hasta 31)
                for (int d = 2; d <= 31; d++)  // empezamos en 2 porque la primera celda de días ya es G (día 1)
                {
                    row6.Append(CreateCell("", 4));
                }

                // Añadir celda final AL (columna 38)
                row6.Append(CreateCell("TOTAL HORAS POR ACT.", 4));

                sheetData.Append(row6);



                // Fila 7 - Números de días (01 a 31)
                var row7 = new Row { RowIndex = 7, Height = 15, CustomHeight = true };
                for (int i = 0; i < 6; i++) row7.Append(CreateCell("", 4));

                int totalDays = DateTime.DaysInMonth(year, month);
                for (int d = 1; d <= 31; d++)
                {
                    string value = d <= totalDays ? d.ToString("00") : ""; // Mostrar número o celda vacía
                    row7.Append(CreateCell(value, 4));
                }
                sheetData.Append(row7);

                // Fila 8 - Letras de días de la semana (D, L, M, ...)
                var row8 = new Row { RowIndex = 8, Height = 15, CustomHeight = true };
                var letras = new[] { "D", "L", "M", "M", "J", "V", "S" };
                for (int i = 0; i < 6; i++) row8.Append(CreateCell("", 4));

                for (int d = 1; d <= 31; d++)
                {
                    string value = d <= totalDays
                        ? letras[(int)new DateTime(year, month, d).DayOfWeek]
                        : ""; // Mostrar letra del día o vacío
                    row8.Append(CreateCell(value, 4));
                }
                sheetData.Append(row8);



                // Reemplazar cliente en celda C4
                //row4.Elements<Cell>().ElementAt(2).CellValue = new CellValue(reportData.TradeName);

                // Reemplazar nombre completo en celda C5
                //row5.Elements<Cell>().ElementAt(2).CellValue = new CellValue($"{reportData.FirstName} {reportData.LastName}");

                uint rowIndex = 9;
                int actividadIndex = 1;
                int diasEnMes = DateTime.DaysInMonth(year, month);

                // Inicializar arreglo de suma por día con 31 entradas
                decimal[] totalPorDia = new decimal[31];

                // Crear filas de actividades
                foreach (var actividad in reportData.Activities)
                {
                    var row = new Row { RowIndex = rowIndex, Height = 24.60, CustomHeight = true };

                    // Arreglo de horas por día con 31 entradas
                    string[] horasPorDia = new string[31];
                    decimal sumaHoras = actividad.HoursQuantity;

                    int dia = actividad.ActivityDate.Day;
                    horasPorDia[dia - 1] = actividad.HoursQuantity.ToString("0.##");
                    totalPorDia[dia - 1] += actividad.HoursQuantity;

                    // Columnas A–F
                    row.Append(
                        CreateCell(actividadIndex.ToString(), 5),                           // A: número
                        CreateCell(actividad.ActivityType.Name, 6),                         // B: tipo actividad
                        CreateCell(actividad.LeaderName, 6),                                // C: líder
                        CreateCell(actividad.RequirementCode ?? "", 6),                     // D: código requerimiento
                        CreateCell(actividad.ActivityDescription, 7),                       // E: descripción
                        CreateCell(sumaHoras.ToString("0.##"), 6)                           // F: total horas por actividad (1 día en este caso)
                    );

                    // Columnas G–AK (31 columnas de días)
                    for (int d = 1; d <= 31; d++)
                    {
                        string valor = horasPorDia[d - 1] ?? "";
                        uint estilo;

                        if (d <= diasEnMes)
                        {
                            var fecha = new DateTime(year, month, d);
                            var dateOnly = DateOnly.FromDateTime(fecha);

                            bool esFinDeSemana = fecha.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
                            bool esFeriado = holidays.Any(h => h.HolidayDate == dateOnly);
                            bool enPermiso = permissionRanges.Any(r => dateOnly >= r.Start && dateOnly <= r.End);

                            estilo = esFeriado
                                ? 13u
                                : enPermiso
                                    ? 14u
                                    : esFinDeSemana
                                        ? 8u
                                        : 6u;
                        }
                        else
                        {
                            // Día ficticio (ej: 31 en abril), sin datos ni formato especial
                            estilo = 16u;
                            valor = "";
                        }

                        row.Append(CreateCell(valor, estilo));
                    }

                    // Columna AL (total otra vez)
                    row.Append(CreateCell(sumaHoras.ToString("0.##"), 6));

                    sheetData.Append(row);
                    rowIndex++;
                    actividadIndex++;
                }


                // === Fila TOTAL por día ===
                var totalRow = new Row { RowIndex = rowIndex, Height = 20, CustomHeight = true };

                // Celda A (texto TOTAL, combinada A–E)
                totalRow.Append(CreateCell("TOTAL", 5));

                // Celdas vacías para B–E (se ignoran porque A–E estarán combinadas)
                for (int i = 0; i < 4; i++)
                    totalRow.Append(CreateCell("", 5));

                // Celda F: total general de horas
                totalRow.Append(CreateCell(totalPorDia.Sum().ToString("0.0"), 11));

                // Columnas G–AK: sumas por día (siempre 31 columnas)
                for (int d = 0; d < 31; d++)
                {
                    string valor = d < diasEnMes
                        ? totalPorDia[d].ToString("0.0")
                        : ""; // Vacío si el día no existe

                    uint estilo;

                    if (d < diasEnMes)
                    {
                        var fecha = new DateTime(year, month, d + 1);
                        var dateOnly = DateOnly.FromDateTime(fecha);

                        bool esFinDeSemana = fecha.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
                        bool esFeriado = holidays.Any(h => h.HolidayDate == dateOnly);
                        bool enPermiso = permissionRanges.Any(r => dateOnly >= r.Start && dateOnly <= r.End);

                        estilo = esFeriado
                            ? 13u
                            : enPermiso
                                ? 14u
                                : esFinDeSemana
                                    ? 8u
                                    : 11u;
                    }
                    else
                    {
                        estilo = 16u; // Estilo estándar para celdas vacías
                    }

                    totalRow.Append(CreateCell(valor, estilo));
                }

                // Celda AL: total otra vez
                totalRow.Append(CreateCell(totalPorDia.Sum().ToString("0.0"), 11));

                sheetData.Append(totalRow);

                // Añadir celda combinada A{rowIndex}:E{rowIndex}
                mergeCells.Append(new MergeCell { Reference = $"A{rowIndex}:E{rowIndex}" });









                // === BLOQUE DE FIRMAS ===
                uint extraStartRow = rowIndex + 6;

                // Fila vacía con celdas combinadas (para estilo)
                var sigRow1 = new Row { RowIndex = extraStartRow };
                sigRow1.Append(CreateCell(""));  // A
                sigRow1.Append(CreateCell(""));  // B (inicio merge B:D)
                sigRow1.Append(CreateCell(""));
                sigRow1.Append(CreateCell(""));

                for (int i = 5; i < 9; i++) // E a H
                    sigRow1.Append(CreateCell(""));

                sigRow1.Append(CreateCell("")); // I (inicio merge I:U)
                for (int i = 10; i <= 21; i++)
                    sigRow1.Append(CreateCell(""));

                sheetData.Append(sigRow1);

                // Merge para estilo personalizado
                mergeCells.Append(new MergeCell { Reference = $"B{extraStartRow}:D{extraStartRow}" });
                mergeCells.Append(new MergeCell { Reference = $"I{extraStartRow}:U{extraStartRow}" });

                // === FILA 2: Elaborado por / Revisado por ===
                var sigRow2 = new Row { RowIndex = extraStartRow + 1 };
                sigRow2.Append(CreateCell(""));  // A
                sigRow2.Append(CreateCell("Elaborado por: " + reportData.FirstName + " " + reportData.LastName, 9));  // B
                sigRow2.Append(CreateCell("", 9));
                sigRow2.Append(CreateCell("", 9));

                for (int i = 5; i < 9; i++) // E a H
                    sigRow2.Append(CreateCell(""));

                sigRow2.Append(CreateCell($"Revisado y Aprobado por: {primerLiderNombre}", 9)); // I
                for (int i = 10; i <= 21; i++)
                    sigRow2.Append(CreateCell("", 9));

                sheetData.Append(sigRow2);

                // Merge
                mergeCells.Append(new MergeCell { Reference = $"B{extraStartRow + 1}:D{extraStartRow + 1}" });
                mergeCells.Append(new MergeCell { Reference = $"I{extraStartRow + 1}:U{extraStartRow + 1}" });

                // === FILA 3: RPS RISK PROCESS SOLUTIONS S.A. (solo B-D) ===
                var sigRow3 = new Row { RowIndex = extraStartRow + 2 };
                sigRow3.Append(CreateCell(""));     // A

                if (reportData.Company == "ISC" )
                {
                    sigRow3.Append(CreateCell("INTEGRITY SOLUTIONS.", 10)); // B
                }
                else 
                {
                    sigRow3.Append(CreateCell("RPS RISK PROCESS SOLUTIONS S.A.", 10)); // B
                }

                sigRow3.Append(CreateCell(""));     // C
                sigRow3.Append(CreateCell(""));     // D
                                                    // E-H vacías
                for (int i = 5; i < 9; i++)
                    sigRow3.Append(CreateCell(""));

                // I-U: Empresa con concatenación
                string empresaTexto = "Empresa: " + reportData.TradeName;
                sigRow3.Append(CreateCell(empresaTexto, 10));

                // Rellenar hasta la columna U
                for (int i = 10; i <= 21; i++)
                    sigRow3.Append(CreateCell(""));

                sheetData.Append(sigRow3);

                // Merge para B-D y I-U
                mergeCells.Append(new MergeCell { Reference = $"B{extraStartRow + 2}:D{extraStartRow + 2}" });
                mergeCells.Append(new MergeCell { Reference = $"I{extraStartRow + 2}:U{extraStartRow + 2}" });












                // Fila inicial del bloque de nomenclatura
                uint nomenclaturaStartRow = extraStartRow + 4;

                // Textos que irán en la columna C
                string[] textosC = { "Vacaciones", "Feriado", "Permiso", "Fines de Semana" };

                // Estilos específicos para cada fila del bloque (columna C)
                uint[] estilosC = { 12, 13, 14, 15 };

                // Estilo vacío (para columna A)
                uint estiloVacio = 0;

                // Estilo para todas las celdas de columna B (título "Nomenclatura" y celdas vacías mergeadas)
                uint estiloNomenclaturaTitulo = 6;

                for (uint i = 0; i < textosC.Length; i++)
                {
                    var row = new Row
                    {
                        RowIndex = nomenclaturaStartRow + i,
                        Height = 18,
                        CustomHeight = true
                    };

                    // Columna A: vacía
                    row.Append(CreateCell("", estiloVacio));

                    // Columna B: siempre con estilo 6, solo la primera con texto
                    row.Append(CreateCell(i == 0 ? "Nomenclatura" : "", estiloNomenclaturaTitulo));

                    // Columna C: texto con estilo individual
                    row.Append(CreateCell(textosC[i], estilosC[i]));

                    sheetData.Append(row);
                }

                // Merge vertical de B (B{start}:B{start+3}) con estilo 6 aplicado en todas esas celdas
                mergeCells.Append(new MergeCell
                {
                    Reference = $"B{nomenclaturaStartRow}:B{nomenclaturaStartRow + 3}"
                });




                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                string logoPath;

                if (reportData.Company == "ISC")
                {
                    logoPath = Path.Combine(basePath, "Templates", "logo-isc.png");
                }
                else if (reportData.Company == "RPS")
                {
                    logoPath = Path.Combine(basePath, "Templates", "LogoRPSIntegrity.png");
                }
                else
                {
                    throw new FileNotFoundException("No se encontró la compañía a la que pertenece");
                }

                if (!File.Exists(logoPath))
                    throw new FileNotFoundException($"No se encontró el logo en: {logoPath}");



                var drawingsPart = worksheetPart.AddNewPart<DrawingsPart>();
                InsertImageToWorksheet(drawingsPart, worksheetPart, logoPath);




                // Guardar hoja y libro
                worksheetPart.Worksheet.Save();
                workbookPart.Workbook.Save();
            }

            return stream.ToArray();
        }


        // ===================
        // Celdas y Filas
        // ===================

        private Cell CreateCell(string value, uint styleIndex = 0)
        {
            return new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(value ?? ""),
                StyleIndex = styleIndex
            };
        }

        private Row CreateRow(uint rowIndex, double height, params string[] values)
        {
            var row = new Row { RowIndex = rowIndex, Height = height, CustomHeight = true };
            foreach (var val in values)
                row.Append(CreateCell(val));
            return row;
        }

        // ===================
        // Columnas
        // ===================

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

        // ===================
        // Estilos
        // ===================

        private Stylesheet CreateStylesheet()
        {
            return new Stylesheet(
                new Fonts(
                    new Font( // 0 - Default (Calibri 11, no negrita)
                        new FontSize { Val = 11 },
                        new FontName { Val = "Calibri" }
                    ),
                    new Font( // 1 - Calibri 24pt, bold
                        new FontSize { Val = 24 },
                        new Bold(),
                        new FontName { Val = "Calibri" }
                    ),
                    new Font( // 2 - Calibri 14pt, bold
                        new FontSize { Val = 14 },
                        new Bold(),
                        new FontName { Val = "Calibri" }
                    ),
                    new Font( // 3 - Calibri 12pt, bold
                        new FontSize { Val = 12 },
                        new Bold(),
                        new FontName { Val = "Calibri" }
                    ),
                    new Font( // 4 - Calibri 9pt, bold
                        new FontSize { Val = 9 },
                        new Bold(),
                        new FontName { Val = "Calibri" }
                    ),
                    new Font( // 5 - Calibri 11pt, bold
                        new FontSize { Val = 11 },
                        new Bold(),
                        new FontName { Val = "Calibri" }
                    ),
                    new Font( // 6 - Calibri 12pt, negrita, azul
                        new FontSize { Val = 12 },
                        new Color { Rgb = "FF0000FF" }, // Azul
                        new Bold(),
                        new FontName { Val = "Calibri" }
                    ),
                    new Font( // 7 - Calibri 12pt, normal (nuevo para tus estilos)
                    new FontSize { Val = 12 },
                    new FontName { Val = "Calibri" }
                    )
                ),

                new Fills(
                    new Fill(new PatternFill { PatternType = PatternValues.None }), // 0
                    new Fill(new PatternFill { PatternType = PatternValues.Gray125 }), // 1
                    new Fill(new PatternFill( // 2 - Gris oscuro 25%
                        new ForegroundColor { Rgb = "FFBFBFBF" })
                    { PatternType = PatternValues.Solid }),
                    new Fill(new PatternFill( // 3 - Amarillo claro
                        new ForegroundColor { Rgb = "FFFFFF7F" })
                    { PatternType = PatternValues.Solid }),
                    new Fill(new PatternFill( // 4 - Azul claro énfasis 1
                        new ForegroundColor { Rgb = "FF8DB4E2" })
                    { PatternType = PatternValues.Solid }),

                    new Fill(new PatternFill( // 5 - Fondo #FFC000 (naranja)
                    new ForegroundColor { Rgb = "FFFFC000" })
                    { PatternType = PatternValues.Solid }),

                    new Fill(new PatternFill( // 6 - Fondo #FFFF00 (amarillo puro)
                        new ForegroundColor { Rgb = "FFFFFF00" })
                    { PatternType = PatternValues.Solid }),

                    new Fill(new PatternFill( // 7 - Fondo #76933C (verde oscuro)
                        new ForegroundColor { Rgb = "FF76933C" })
                    { PatternType = PatternValues.Solid }),

                    new Fill(new PatternFill( // 8 - Fondo #8DB4E2 (azul claro)
                        new ForegroundColor { Rgb = "FF8DB4E2" })
                    { PatternType = PatternValues.Solid }),

                    new Fill(new PatternFill( // 9 - Fondo completamente negro
                    new ForegroundColor { Rgb = "FF000000" }) // Negro
                    { PatternType = PatternValues.Solid })


                ),

                new Borders(
                    new Border(), // 0 - Sin bordes
                    new Border(   // 1 - Bordes completos
                        new LeftBorder { Style = BorderStyleValues.Thin },
                        new RightBorder { Style = BorderStyleValues.Thin },
                        new TopBorder { Style = BorderStyleValues.Thin },
                        new BottomBorder { Style = BorderStyleValues.Thin },
                        new DiagonalBorder()),
                    new Border( // 2 - Borde solo arriba
                    new LeftBorder(),
                    new RightBorder(),
                    new TopBorder { Style = BorderStyleValues.Thin },
                    new BottomBorder(),
                    new DiagonalBorder())

                ),

                new CellFormats(
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 0 }, // 0 - Default

                    new CellFormat // 1 - 24pt, centrado
                    {
                        FontId = 1,
                        FillId = 0,
                        BorderId = 0,
                        Alignment = new Alignment
                        {
                            Horizontal = HorizontalAlignmentValues.Center,
                            Vertical = VerticalAlignmentValues.Center,
                            WrapText = true
                        },
                        ApplyFont = true,
                        ApplyAlignment = true
                    },

                    new CellFormat // 2 - 14pt, centrado
                    {
                        FontId = 2,
                        FillId = 0,
                        BorderId = 0,
                        Alignment = new Alignment
                        {
                            Horizontal = HorizontalAlignmentValues.Center,
                            Vertical = VerticalAlignmentValues.Center,
                            WrapText = true
                        },
                        ApplyFont = true,
                        ApplyAlignment = true
                    },

                    new CellFormat // 3 - 12pt, alineado a la izquierda
                    {
                        FontId = 3,
                        FillId = 0,
                        BorderId = 0,
                        Alignment = new Alignment
                        {
                            Horizontal = HorizontalAlignmentValues.Left,
                            Vertical = VerticalAlignmentValues.Center,
                            WrapText = true
                        },
                        ApplyFont = true,
                        ApplyAlignment = true
                    },

                    new CellFormat // 4 - 9pt, gris oscuro 25%, bordes, centrado
                    {
                        FontId = 4,
                        FillId = 2,
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

                    new CellFormat // 5 - 11pt, gris oscuro 25%, bordes, centrado
                    {
                        FontId = 5,
                        FillId = 2,
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

                    new CellFormat // 6 - 11pt, sin fondo, bordes, centrado
                    {
                        FontId = 5,
                        FillId = 0,
                        BorderId = 1,
                        Alignment = new Alignment
                        {
                            Horizontal = HorizontalAlignmentValues.Center,
                            Vertical = VerticalAlignmentValues.Center,
                            WrapText = true
                        },
                        ApplyFont = true,
                        ApplyBorder = true,
                        ApplyAlignment = true
                    },

                    new CellFormat // 7 - 11pt, amarillo claro, bordes, centrado
                    {
                        FontId = 5,
                        FillId = 3,
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

                    new CellFormat // 8 - 11pt, azul claro, bordes, centrado
                    {
                        FontId = 5,
                        FillId = 4,
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

                    new CellFormat // 9 - 11pt, borde arriba, centrado, negrita
                    {
                        FontId = 5, // Calibri 11, negrita
                        FillId = 0,
                        BorderId = 2, // borde solo arriba
                        Alignment = new Alignment
                        {
                            Horizontal = HorizontalAlignmentValues.Center,
                            Vertical = VerticalAlignmentValues.Center,
                            WrapText = true
                        },
                        ApplyFont = true,
                        ApplyBorder = true,
                        ApplyAlignment = true
                    },

                    new CellFormat // 10 - 11pt, centrado, negrita, sin bordes
                    {
                        FontId = 5,
                        FillId = 0,
                        BorderId = 0,
                        Alignment = new Alignment
                        {
                            Horizontal = HorizontalAlignmentValues.Center,
                            Vertical = VerticalAlignmentValues.Center,
                            WrapText = true
                        },
                        ApplyFont = true,
                        ApplyAlignment = true
                    },

                    new CellFormat // 11 - 12pt azul, centrado, bordes completos
                    {
                        FontId = 6,
                        FillId = 0,
                        BorderId = 1, // bordes completos
                        Alignment = new Alignment
                        {
                            Horizontal = HorizontalAlignmentValues.Center,
                            Vertical = VerticalAlignmentValues.Center,
                            WrapText = true
                        },
                        ApplyFont = true,
                        ApplyBorder = true,
                        ApplyAlignment = true
                    },

                    new CellFormat // 12 - Calibri 12, fondo #naranja, bordes completos, centrado
                    {
                        FontId = 7, // Calibri 12 normal
                        FillId = 5, // #FFC000
                        BorderId = 1, // Bordes completos
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

                    new CellFormat // 13 - Calibri 12, fondo #amarillo, bordes completos, centrado
                    {
                        FontId = 7,
                        FillId = 6,
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

                    new CellFormat // 14 - Calibri 12, fondo #verde, bordes completos, centrado
                    {
                        FontId = 7,
                        FillId = 7,
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

                    new CellFormat // 15 - Calibri 12, fondo #celeste, bordes completos, centrado
                    {
                        FontId = 7,
                        FillId = 8,
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

                    new CellFormat // 16 - Fondo negro, texto negro, bordes, centrado
                    {
                        FontId = 0,       // Texto negro (por defecto)
                        FillId = 9,       // Fondo negro
                        BorderId = 1,     // Bordes completos
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
                    }



                )
            );
        }


        // ===================
        // Utilidades de fecha y columnas
        // ===================

        private string GetMonthName(int month)
        {
            return new CultureInfo("es-ES").DateTimeFormat.GetMonthName(month);
        }

        private int GetColumnIndexFromCellReference(string cellRef)
        {
            if (string.IsNullOrEmpty(cellRef))
                return 0;

            int colIdx = 0;
            foreach (char ch in cellRef)
            {
                if (char.IsLetter(ch))
                {
                    colIdx = colIdx * 26 + (char.ToUpper(ch) - 'A' + 1);
                }
                else
                {
                    break;
                }
            }
            return colIdx;
        }

        private bool IsWeekendColumn(int colIdx, int year, int month)
        {
            int day = colIdx - 6; // Columna 7 (G) = día 1
            if (day < 1 || day > DateTime.DaysInMonth(year, month))
                return false;

            var dayOfWeek = new DateTime(year, month, day).DayOfWeek;
            return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
        }


        public async Task<TimeReportDataFillDto> GetTimeReportDataFillAsync(int employeeId, int clientId, int year, int month, bool fullMonth)
        {
            var employee = await employeeRepository.GetEmployeeByIDAsync(employeeId);
            if (employee?.Person == null)
                throw new Exception("Empleado o datos personales no encontrados.");

            var client = await clientRepository.GetClientByIDAsync(clientId);
            if (client == null)
                throw new Exception("Cliente no encontrado.");

            //ESTO DE AQUI ES COMO SI VA A IR     var projectIds = await timeReportRepository.GetProjectIdsForEmployeeByClientAsync(employeeId, clientId);

            //ESTO HAY QUE CAMBIARLO EN UN FUTURO PORQUE N DEBERIA DE SER ASI PERO HCIIERO  UQ ELO HAGAMOS ASI YA PUES ASI GTOCA

            var projectIds = await projectRepository.GetProjectToEmployeeAsync(employeeId);


            if (projectIds == null || !projectIds.Any())
            {
                return new TimeReportDataFillDto
                {
                    FirstName = employee.Person.FirstName,
                    LastName = employee.Person.LastName,
                    TradeName = client.TradeName ?? string.Empty,
                    Activities = new List<TimeReportActivityDto>()
                };
            }

            var activities = await timeReportRepository.GetActivitiesByEmployeeAndProjectsAsync(
                employeeId, projectIds, year, month, fullMonth) ?? new List<DailyActivity>();

            var leaders = await leaderRepository.GetActiveLeadersByProjectIdsAsync(projectIds);

            var activityDtos = activities.Select(a =>
            {
                var leader = leaders.FirstOrDefault(l => l.ProjectID == a.ProjectID);
                var leaderName = leader != null ? $"{leader.Person.FirstName} {leader.Person.LastName}" : string.Empty;

                return new TimeReportActivityDto
                {
                    LeaderId = leader?.PersonID ?? 0,
                    LeaderName = leaderName,
                    ActivityTypeID = a.ActivityTypeID,
                    ActivityType = a.ActivityType,
                    HoursQuantity = a.HoursQuantity,
                    ActivityDate = a.ActivityDate,
                    ActivityDescription = a.ActivityDescription,
                    Notes = a.Notes,
                    RequirementCode = a.RequirementCode
                };
            }).ToList();

            return new TimeReportDataFillDto
            {
                FirstName = employee.Person.FirstName,
                LastName = employee.Person.LastName,
                TradeName = client.TradeName ?? string.Empty,
                Company = client.Company,
                Activities = activityDtos,
                EmployeeCompany = employee.ContractType
            };
        }
        private void InsertImageToWorksheet(DrawingsPart drawingsPart, WorksheetPart worksheetPart, string imagePath)
        {
            using var imageStream = File.OpenRead(imagePath);
            var imagePart = drawingsPart.AddImagePart(ImagePartType.Png);
            imagePart.FeedData(imageStream);

            // Crear el contenedor de dibujo si no existe
            if (drawingsPart.WorksheetDrawing == null)
            {
                drawingsPart.WorksheetDrawing = new Xdr.WorksheetDrawing();
            }

            // Crear relación entre hoja y dibujo si no existe
            if (!worksheetPart.Worksheet.Elements<Drawing>().Any())
            {
                var drawing = new Drawing { Id = worksheetPart.GetIdOfPart(drawingsPart) };
                worksheetPart.Worksheet.Append(drawing);
            }

            var fromColumn = 34; // AX
            var fromRow = 0;    // Fila 100
            var toColumn = fromColumn + 4; // termina en BA (53)
            var toRow = fromRow + 3;       // termina en fila 106

            if (imagePath != null && imagePath.Contains("logo-isc.png", StringComparison.OrdinalIgnoreCase))
            {
                toColumn = fromColumn + 3;
            }

            var nvps = new Xdr.NonVisualPictureProperties(
                new Xdr.NonVisualDrawingProperties { Id = 0U, Name = "Logo" },
                new Xdr.NonVisualPictureDrawingProperties(new A.PictureLocks { NoChangeAspect = true })
            );

            var blipFill = new Xdr.BlipFill(
                new A.Blip { Embed = drawingsPart.GetIdOfPart(imagePart), CompressionState = A.BlipCompressionValues.Print },
                new A.Stretch(new A.FillRectangle())
            );

            var shapeProps = new Xdr.ShapeProperties(
                new A.Transform2D(
                    new A.Offset { X = 0, Y = 0 },
                    new A.Extents { Cx = 990000L, Cy = 792000L }),
                new A.PresetGeometry(new A.AdjustValueList()) { Preset = A.ShapeTypeValues.Rectangle }
            );

            var picture = new Xdr.Picture(nvps, blipFill, shapeProps);

            var anchor = new Xdr.TwoCellAnchor(
                new Xdr.FromMarker(
                    new Xdr.ColumnId(fromColumn.ToString()),
                    new Xdr.ColumnOffset("0"),
                    new Xdr.RowId(fromRow.ToString()),
                    new Xdr.RowOffset("0")),
                new Xdr.ToMarker(
                    new Xdr.ColumnId(toColumn.ToString()),
                    new Xdr.ColumnOffset("0"),
                    new Xdr.RowId(toRow.ToString()),
                    new Xdr.RowOffset("0")),
                picture,
                new Xdr.ClientData()
            );

            drawingsPart.WorksheetDrawing.Append(anchor);
            drawingsPart.WorksheetDrawing.Save();
        }

        public async Task<List<DashboardRecursosPendientesDto>> GetRecursosTimeReportPendienteAsync(int? month = null, int? year = null, bool mesCompleto = false)
        {
            return await timeReportRepository.GetRecursosTimeReportPendienteAsync(month, year, mesCompleto);
        }

    }
}
