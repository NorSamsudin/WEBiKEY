using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using OfficeOpenXml;

namespace WEBiKEY.Application.Classes
{
    public class ExcelUtils
    {
        public static string IGNORE_COLUMN_VALUE = "--(Ignore)--";


        /// <summary>
        /// Given a cell name, parses the specified cell to get the column name.
        /// </summary>
        /// <param name="cellReference">Address of the cell (eg. B2)</param>
        /// <returns>Column Name (eg. B)</returns>
        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }

        ///// <summary>
        ///// Given just the column name (no row index), it will return the zero based column index.
        ///// Note: This method will only handle columns with a length of up to two (ie. A to Z and AA to ZZ). 
        ///// A length of three can be implemented when needed.
        ///// </summary>
        ///// <param name="columnName">Column Name (ie. A or AB)</param>
        ///// <returns>Zero based index if the conversion was successful; otherwise null</returns>
        //public static int GetColumnIndexFromName(string columnName)
        //{
        //    int columnIndex = 0;

        //    string[] colLetters = Regex.Split(columnName, "([A-Z]+)");
        //    colLetters = colLetters.Where(s => !string.IsNullOrEmpty(s)).ToArray();

        //    if (colLetters.Count() < 2)
        //    {
        //        char[] colCharletters = colLetters.First().ToCharArray();

        //        int index = 0;
        //        foreach (char colChar in colCharletters)
        //        {
        //            columnIndex += 26 * (colCharletters.Count() - index - 1) + (Convert.ToInt16(colChar) - 65);
        //            //List<char> col1 = colLetters.ElementAt(index).ToCharArray().ToList();
        //            //int? indexValue = Letters.IndexOf(col1.ElementAt(index));

        //            //if (indexValue != -1)
        //            //{
        //            //    // The first letter of a two digit column needs some extra calculations
        //            //    if (index == 0 && colLetters.Count() == 2)
        //            //    {
        //            //        columnIndex = columnIndex == null ? (indexValue + 1) * 26 : columnIndex + ((indexValue + 1) * 26);
        //            //    }
        //            //    else
        //            //    {
        //            //        columnIndex = columnIndex == null ? indexValue : columnIndex + indexValue;
        //            //    }
        //            //}

        //            index++;
        //        }
        //    }

        //    return columnIndex;
        //}

        ///// <summary>
        ///// Returns the value of the given Excel Cell
        ///// </summary>
        ///// <param name="document">Reference to the Excel Document</param>
        ///// <param name="cell">Reference to the Excel Cell with the value</param>
        ///// <returns>String value of the cell contents</returns>
        //public static string GetCellValue(ExcelPackage document, Cell cell)
        //{
        //    SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
        //    string value = "";

        //    if (cell.CellValue != null)
        //    {
        //        value = cell.CellValue.InnerXml;
        //    }

        //    if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
        //    {
        //        return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
        //    }
        //    else
        //    {
        //        return value;
        //    }
        //}

        /// <summary>
        /// Get the Excel row value for the given column
        /// </summary>
        /// <param name="row">DataRow representing the Excel Row</param>
        /// <param name="columnName">Must be in UPPER CASE</param>
        /// <param name="columnMap">Database and Excel Column Map to search for</param>
        /// <returns></returns>
        public static object GetExcelRowValueForColumn(DataRow row, string columnName, Dictionary<string, string> columnMap)
        {
            string excelColName = GetExcelColumnName(columnName, columnMap);

            return GetExcelRowValueForColumn(row, excelColName);
        }

        /// <summary>
        /// Get the Excel cell value for the given column
        /// </summary>
        /// <param name="row">DataRow representing the Excel Row</param>
        /// <param name="columnName">Column Name in Row</param>
        /// <returns></returns>
        public static object GetExcelRowValueForColumn(DataRow row, string columnName)
        {
            if (!string.IsNullOrEmpty(columnName))
            {
                if (row[columnName] != null && row[columnName].ToString().Trim() != "")
                {
                    string val = row[columnName].ToString().Trim();
                    if (!string.IsNullOrEmpty(val))
                    {
                        return row[columnName];
                    }
                }
            }

            return string.Empty;
        }


        /// <summary>
        /// Get the Excel column name from the provided Map
        /// </summary>
        /// <param name="dbColumnName">DB Column name (KEY) in the Map</param>
        /// <param name="columnMap">Dictionary of DB column names and Excel column names</param>
        /// <returns>Corresponding Excel column name for the dbColumnName</returns>
        public static string GetExcelColumnName(string dbColumnName, Dictionary<string, string> columnMap)
        {
            string excelColName = "";

            if (columnMap.Keys.Contains(dbColumnName.ToUpper()))
            {
                excelColName = columnMap[dbColumnName.ToUpper()];
            }

            return excelColName;
        }

        ///// <summary>
        ///// Get all rows in the spreadsheet
        ///// </summary>
        ///// <param name="spreadSheetDocument">Reference to the SpreadsheetDocument</param>
        ///// <returns>Collection of Rows in spreadsheet</returns>
        //public static IEnumerable<Row> GetExcelRows(ExcelPackage spreadSheetDocument)
        //{
        //    IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
        //    string relationshipId = sheets.First().Id.Value;
        //    WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
        //    Worksheet workSheet = worksheetPart.Worksheet;
        //    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
        //    IEnumerable<Row> rows = sheetData.Descendants<Row>();
        //    return rows;
        //}

        public static IEnumerable<string> GetExcelHeaderNames(ExcelPackage excelPackage)
        {
            ExcelWorksheet xlWorksheet = excelPackage.Workbook.Worksheets.First();

            int endColIdx = xlWorksheet.Dimension.Columns;
            var xlFirstRow = xlWorksheet.Cells[1, 1, 1, endColIdx];

            List<string> headersList = xlFirstRow.Select(firstRowCell => firstRowCell.Text).ToList();

            return headersList;
        }

        /// <summary>
        /// Convert Excel data to DataTable
        /// </summary>
        /// <param name="spreadSheetDocument">Reference to SpreadsheetDocument</param>
        /// <returns>DataTable containing Excel data</returns>
        public static DataTable GetDataTableFromExcel(ExcelPackage xlPkg, bool skipBlankRows)
        {
            ExcelWorksheet xlWorksheet = xlPkg.Workbook.Worksheets.First();

            DataTable dataTable = new DataTable();

            int endColIdx = xlWorksheet.Dimension.Columns;
            var xlFirstRow = xlWorksheet.Cells[1, 1, 1, endColIdx];

            foreach (var firstRowCell in xlFirstRow)
            {
                dataTable.Columns.Add(firstRowCell.Text);
            }

            var startRow = 2;

            if (skipBlankRows)
            {
                for (int rowNum = startRow; rowNum <= xlWorksheet.Dimension.Rows; rowNum++)
                {
                    var wsRow = xlWorksheet.Cells[rowNum, 1, rowNum, endColIdx];

                    bool isBlank = wsRow.All(cell => string.IsNullOrEmpty(cell.Text));

                    if (isBlank)
                    {
                        continue;
                    }

                    DataRow row = dataTable.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        if (cell.Start.Column <= dataTable.Columns.Count)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    }
                }
            }
            else
            {
                for (int rowNum = startRow; rowNum <= xlWorksheet.Dimension.Rows; rowNum++)
                {
                    var wsRow = xlWorksheet.Cells[rowNum, 1, rowNum, endColIdx];

                    DataRow row = dataTable.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        if (cell.Start.Column <= dataTable.Columns.Count)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    }
                }
            }

            return dataTable;

        }
    }
}