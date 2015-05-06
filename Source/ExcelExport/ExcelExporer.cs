namespace ExcelExport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;

    public class ExcelExporer : IExcelExporer
    {
        private uint rowIndex;
        private string templatePath;
        private string copyPath;
        private IList<Row> rows;

        public ExcelExporer(string templatePath, string copyPath)
        {
            this.templatePath = templatePath;
            this.copyPath = copyPath;

            this.RowIndex = 1;
            this.rows = new List<Row>();

            this.CopyTemplate();
        }

        public uint RowIndex
        {
            get
            {
                return this.rowIndex;
            }
            protected set
            {
                if (value > 0)
                {
                    this.rowIndex = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("rowIndex", "RowIndex mast be greater then 0!");
                }
            }
        }

        public void CreateExcelDocument()
        {
            this.AppendRowsToDocument(this.rows);
        }

        public void AppendToDocument(IList<ExcelCell> rowCells)
        {
            var cells = new List<Cell>();

            for (int i = 0; i < rowCells.Count; i++)
            {
                var currentCell = rowCells[i].GetCell(this.RowIndex);
                cells.Add(currentCell);
            }

            var row = this.CreateContentRow(cells);
            this.rows.Add(row);
        }

        protected void CopyTemplate()
        {
            //Make a copy of the template file.
            File.Copy(this.templatePath, this.copyPath, true);
        }

        private void AppendRowsToDocument(IEnumerable<Row> rows)
        {
            //Open the copied template workbook. 
            using (SpreadsheetDocument myWorkbook = SpreadsheetDocument.Open(this.copyPath, true))
            {
                //Access the main Workbook part, which contains all references.
                WorkbookPart workbookPart = myWorkbook.WorkbookPart;
                //Get the first worksheet. 
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                // The SheetData object will contain all the data.
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                foreach (var row in rows)
                {
                    //Append new row to sheet data.
                    sheetData.AppendChild(row);
                }
            }
        }

        private Row CreateContentRow(IEnumerable<Cell> cells)
        {
            //Create the new row.
            Row row = new Row();
            row.RowIndex = this.RowIndex;

            foreach (var cell in cells)
            {
                row.AppendChild(cell);
            }

            this.RowIndex += 1;

            return row;
        }
    }
}
