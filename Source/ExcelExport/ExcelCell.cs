namespace ExcelExport
{
    using System;

    using DocumentFormat.OpenXml.Spreadsheet;

    public abstract class ExcelCell
    {
        private string cellData;
        private string headerName;

        public ExcelCell(string cellData, string headerName)
        {
            this.CellData = cellData;
            this.HeaderName = headerName;
        }

        public string CellData
        {
            get { return cellData; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("cellData", "CellData can not be null!");
                }

                cellData = value;
            }
        }

        public string HeaderName
        {
            get { return headerName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("HeaderName", "Can not be null or white space!");
                }

                headerName = value;
            }
        }

        internal abstract Cell GetCell(uint rowIndex);
    }
}
