namespace ExcelExport
{
    using DocumentFormat.OpenXml.Spreadsheet;

    public class TextCell : ExcelCell
    {
        public TextCell(string cellData, string headerName)
            : base(cellData, headerName)
        {

        }

        internal override Cell GetCell(uint rowIndex)
        {
            //Create a new inline string cell.
            Cell cell = new Cell();
            cell.DataType = CellValues.InlineString;
            cell.CellReference = this.HeaderName + rowIndex;
            //Add text to the text cell.
            InlineString inlineString = new InlineString();
            Text text = new Text();

            text.Text = this.CellData;
            inlineString.AppendChild(text);
            cell.AppendChild(inlineString);

            return cell;
        }
    }
}
