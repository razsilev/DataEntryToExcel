namespace ExcelExport
{
    using System.Collections.Generic;

    public interface IExcelExporer
    {
        uint RowIndex { get; }

        void CreateExcelDocument();

        void AppendToDocument(IList<ExcelCell> rowCells);
    }
}
