using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using isc.time.report.be.application.Interfaces.Service.TimeReports;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Services.TimeReports
{
    public class TimeReportService : ITimeReportService
    {
        public async Task<byte[]> GenerateExcelReportAsync(int employeeId, int year, int month)
        {
            using var stream = new MemoryStream();

            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.MacroEnabledWorkbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylesPart.Stylesheet = CreateStylesheet();
                stylesPart.Stylesheet.Save();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                var worksheet = new Worksheet();

                // Columnas
                var columns = new Columns();
                columns.Append(CreateColumn(1, 1, 3.56));     // A
                columns.Append(CreateColumn(2, 2, 18.44));    // B
                columns.Append(CreateColumn(3, 3, 18.11));    // C
                columns.Append(CreateColumn(4, 4, 21.22));    // D
                columns.Append(CreateColumn(5, 5, 63.44));    // E
                columns.Append(CreateColumn(6, 6, 9.22));     // F
                for (uint i = 7; i <= 37; i++) columns.Append(CreateColumn(i, i, 3.56));
                columns.Append(CreateColumn(38, 38, 10.11));  // AL

                worksheet.Append(columns);
                worksheet.Append(sheetData);

                // Merge cells
                var mergeCells = new MergeCells(
                    new MergeCell { Reference = "A1:AM1" },
                    new MergeCell { Reference = "F2:H2" },
                    new MergeCell { Reference = "I2:J2" },
                    new MergeCell { Reference = "A4:B4" },
                    new MergeCell { Reference = "C4:D4" },
                    new MergeCell { Reference = "A5:B5" },
                    new MergeCell { Reference = "C5:D5" },
                    new MergeCell { Reference = "G6:AK6" },
                    new MergeCell { Reference = "A6:A8" },
                    new MergeCell { Reference = "B6:B8" },
                    new MergeCell { Reference = "C6:C8" },
                    new MergeCell { Reference = "D6:D8" },
                    new MergeCell { Reference = "E6:E8" },
                    new MergeCell { Reference = "F6:F8" }
                );
                worksheet.Append(mergeCells);

                worksheetPart.Worksheet = worksheet;

                // Hoja
                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                sheets.Append(new Sheet
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "TimeReport"
                });

                // Fila 1: tamaño 24 (estilo 4)
                var row1 = new Row { RowIndex = 1, Height = 21, CustomHeight = true };
                row1.Append(CreateCell("TIME REPORT", 4));
                sheetData.Append(row1);

                // Fila 2: tamaño 24 (estilo 4)
                var row2 = new Row { RowIndex = 2, Height = 18, CustomHeight = true };
                row2.Append(
                    CreateCell("", 4), CreateCell("", 4), CreateCell("", 4), CreateCell("", 4), CreateCell("", 4),
                    CreateCell("", 4), CreateCell("", 4), CreateCell(GetMonthName(month), 4), CreateCell("", 4),
                    CreateCell(year.ToString(), 4)
                );
                sheetData.Append(row2);

                // Fila 3 (vacía, tamaño 12, estilo 3)
                var row3 = new Row { RowIndex = 3, Height = 18, CustomHeight = true };
                sheetData.Append(row3); // vacía sin celdas

                // Fila 4: tamaño 12 (estilo 3)
                var row4 = new Row { RowIndex = 4, Height = 18, CustomHeight = true };
                row4.Append(CreateCell("Cliente:", 3), CreateCell("", 3), CreateCell("Nombre del Cliente", 3));
                sheetData.Append(row4);

                // Fila 5: tamaño 12 (estilo 3)
                var row5 = new Row { RowIndex = 5, Height = 16.8, CustomHeight = true };
                row5.Append(CreateCell("Nombre del Consultor:", 3), CreateCell("", 3), CreateCell("Juan Pérez", 3));
                sheetData.Append(row5);

                // Fila 6: tamaño 9 (estilo 1 para columnas 6-8, 0 para otras)
                var row6 = new Row { RowIndex = 6, Height = 15, CustomHeight = true };
                row6.Append(
                    CreateCell("N°", 1), CreateCell("Tipo de actividad", 1), CreateCell("Líder de proyecto", 1),
                    CreateCell("Código requerimiento / incidente", 1),
                    CreateCell("Descripción de trabajos realizados", 1),
                    CreateCell("Total horas por actividad", 1),
                    CreateCell("Distribución de tiempo en el día", 1)
                );
                sheetData.Append(row6);

                // Fila 7: tamaño 9
                var row7 = new Row { RowIndex = 7, Height = 15, CustomHeight = true };
                for (int i = 0; i < 6; i++) row7.Append(CreateCell("", 0));
                for (int d = 1; d <= DateTime.DaysInMonth(year, month); d++)
                    row7.Append(CreateCell(d.ToString("00"), 1));
                sheetData.Append(row7);

                // Fila 8: tamaño 9
                var row8 = new Row { RowIndex = 8, Height = 15, CustomHeight = true };
                for (int i = 0; i < 6; i++) row8.Append(CreateCell("", 0));
                var letras = new[] { "D", "L", "M", "M", "J", "V", "S" };
                for (int d = 1; d <= DateTime.DaysInMonth(year, month); d++)
                {
                    var dia = new DateTime(year, month, d).DayOfWeek;
                    row8.Append(CreateCell(letras[(int)dia], 1));
                }
                sheetData.Append(row8);

                // Fila 9: tamaño 9, azul para fines de semana, gris para otros
                var row9 = new Row { RowIndex = 9, Height = 24.60, CustomHeight = true };

                // Celdas A a F (sin fondo)
                row9.Append(
                    CreateCell("1", 0),  // A
                    CreateCell("", 0),   // B
                    CreateCell("", 0),   // C
                    CreateCell("", 0),   // D
                    CreateCell("", 0),   // E
                    CreateCell("0", 0)   // F
                );

                // Celdas G hasta AK (01 al 31)
                for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
                {
                    var date = new DateTime(year, month, day);
                    var isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
                    row9.Append(CreateCell("0", isWeekend ? 2u : 0u)); // estilo 2 azul si es fin de semana
                }

                // Celda AL (Total mes)
                row9.Append(CreateCell("0", 0));

                sheetData.Append(row9);

                worksheetPart.Worksheet.Save();
                workbookPart.Workbook.Save();
            }

            return stream.ToArray();
        }


        // Helper: detectar si columna es fin de semana (para simplificar, columnas 7 y 8 cada semana son S y D)
        private bool IsWeekendColumn(int columnIndex)
        {
            if (columnIndex < 7) return false; // antes de la columna 7 no es fin de semana

            // Columnas 7 y 8 (G y H) cada semana son sábado y domingo
            // El patrón se repite cada 7 columnas
            int mod = (columnIndex - 7) % 7;
            return mod == 0 || mod == 1; // sábado o domingo
        }



        private Row CreateRow(uint rowIndex, double height, params string[] values)
        {
            var row = new Row { RowIndex = rowIndex, Height = height, CustomHeight = true };
            foreach (var val in values)
            {
                row.Append(CreateCell(val));
            }
            return row;
        }

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

        private string GetMonthName(int month)
        {
            return new CultureInfo("es-ES").DateTimeFormat.GetMonthName(month);
        }



        private Stylesheet CreateStylesheet()
        {
            return new Stylesheet(
                new Fonts(
                    new Font( // 0 - Calibri 9pt negrita
                        new FontSize { Val = 9 },
                        new Bold(),
                        new FontName { Val = "Calibri" }
                    ),
                    new Font( // 1 - Calibri 12pt negrita
                        new FontSize { Val = 12 },
                        new Bold(),
                        new FontName { Val = "Calibri" }
                    ),
                    new Font( // 2 - Calibri 24pt negrita
                        new FontSize { Val = 24 },
                        new Bold(),
                        new FontName { Val = "Calibri" }
                    )
                ),
                new Fills(
                    new Fill(new PatternFill { PatternType = PatternValues.None }), // 0
                    new Fill(new PatternFill { PatternType = PatternValues.Gray125 }), // 1
                    new Fill(new PatternFill(
                        new ForegroundColor { Rgb = "FFD9D9D9" })
                    { PatternType = PatternValues.Solid }), // 2 - Gris
                    new Fill(new PatternFill(
                        new ForegroundColor { Rgb = "FFDCE6F1" })
                    { PatternType = PatternValues.Solid })  // 3 - Azul claro
                ),

                new Borders(
                    new Border() // 0 - Sin bordes
                ),

                new CellFormats(
                    new CellFormat // 0 - Default
                    {
                        FontId = 0,
                        FillId = 0,
                        BorderId = 0,
                        ApplyFont = true
                    },
                    new CellFormat // 1 - 9pt, negrita, gris claro, borde y centrado
                    {
                        FontId = 0,
                        FillId = 2,
                        BorderId = 0,
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
                    new CellFormat // 2 - 9pt, negrita, azul claro, borde y centrado
                    {
                        FontId = 0,
                        FillId = 3,
                        BorderId = 0,
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
                    new CellFormat // 3 - 12pt, negrita, borde y centrado
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
                        ApplyBorder = true,
                        ApplyAlignment = true
                    },
                    new CellFormat // 4 - 24pt, negrita, borde y centrado
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
                        ApplyBorder = true,
                        ApplyAlignment = true
                    }
                )
            );
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
                    // Convierte la letra mayúscula a índice: A=1, B=2, ...
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
            int day = colIdx - 6; // columna 7 = día 1
            if (day < 1 || day > DateTime.DaysInMonth(year, month))
                return false;

            var dayOfWeek = new DateTime(year, month, day).DayOfWeek;
            return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
        }


    }
}
