namespace ExcelExport
{
    using DocumentFormat.OpenXml.Spreadsheet;

    public class DataCell : ExcelCell
    {
        public DataCell(string cellData, string headerName)
            : base(cellData, headerName)
        {

        }

        internal override Cell GetCell(uint rowIndex)
        {
            //Create a new inline string cell.
            Cell cell = new Cell();

            // set cell coordinates
            cell.CellReference = this.HeaderName + rowIndex;

            CellValue value = new CellValue(this.CellData);

            cell.AppendChild(value);

            return cell;
        }
    }
}
