namespace LibrariesTranscription.ExportToExcel
{
    using System.Collections.Generic;
    
    using ExcelExport;
    using LibrariesTranscription.Data;
    using LibrariesTranscription.Model;

    public class ExportToExcel
    {
        private static string[] headerColumns = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" };

        public static void Main()
        {
            List<LibraryItem> items = GetItems();
            MakeExcelExport(items, "template.xlsx", "LibrariesStateColoradoData.xlsx");
        }

        private static void MakeExcelExport(List<LibraryItem> items, string templatePath, string reportPath)
        {
            ExcelExporer exporter = new ExcelExporer(templatePath, reportPath);
            
            for (int i = 0; i < items.Count; i++)
            {
                List<ExcelCell> cellsOnRow = CreateRowCells(items[i]);

                exporter.AppendToDocument(cellsOnRow);
            }

            exporter.CreateExcelDocument();
        }

        private static List<ExcelCell> CreateRowCells(LibraryItem libraryItem)
        {
            List<ExcelCell> cells = new List<ExcelCell>();

            cells.Add(new TextCell(libraryItem.Organization, headerColumns[0]));
            cells.Add(new TextCell(libraryItem.Name, headerColumns[1]));
            cells.Add(new TextCell(libraryItem.Library, headerColumns[2]));
            cells.Add(new TextCell(libraryItem.Type, headerColumns[3]));
            cells.Add(new TextCell(libraryItem.AddressLine1, headerColumns[4]));
            cells.Add(new TextCell(libraryItem.AddressLine2, headerColumns[5]));
            cells.Add(new TextCell(libraryItem.City, headerColumns[6]));
            cells.Add(new TextCell(libraryItem.State, headerColumns[7]));

            cells.Add(new DataCell(libraryItem.ZipCode, headerColumns[8]));

            cells.Add(new TextCell(libraryItem.ZipCode4, headerColumns[9]));
            cells.Add(new TextCell(libraryItem.County, headerColumns[10]));
            cells.Add(new TextCell(libraryItem.LibraryPhoneNumber, headerColumns[11]));
            
            return cells;
        }

        private static List<LibraryItem> GetItems()
        {
            LibraruDbContext context = new LibraruDbContext();
            var database = new Database(context);

            return database.All();
        }
    }
}
