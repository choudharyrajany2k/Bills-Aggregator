using ClosedXML.Excel;
using System.Linq;

namespace Extract.Excel.Loader.Extensions
{
    public static class ExcelExtensionMethods
    {
        public static string GetCoulmnLetter(this IXLWorksheet ws, string columnName)
        {
            var cellsUsed = ws.RangeUsed().FirstRowUsed().CellsUsed(y => y.Value.ToString() == columnName).ToList();
            return cellsUsed.FirstOrDefault()?.Address.ColumnLetter ?? string.Empty;
        }

        public static int RowsInUse(this IXLWorksheet ws)
        {
            return ws.RangeUsed().RangeAddress.RowSpan;
        }

        public static int ColumnsInUse(this IXLWorksheet ws)
        {
            return ws.RangeUsed().RangeAddress.ColumnSpan;
        }
    }
}
