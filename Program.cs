
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Extract.Excel.Loader.Extensions;
using System.Reflection;

namespace Neft.Aggregation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var path = @"C:\ddddddddddddddddddd\AggregateBills\Bills";

            var path = @"C:\ddddddddddddddddddd\AggregateBills\Bills\Chaned Format";


            string[] files = Directory.GetFiles(path, "*.xlsx", SearchOption.AllDirectories);
            var data = new List<Bills>();
            var headers = Bills.GetExcelColumnHeaders();
            foreach (var item in files)
            {
                using (var workBook = new XLWorkbook(item))
                {
                    var workSheet = workBook.Worksheets.Worksheet(1);
                    var rows = workSheet.RowsInUse();
                    for (int i = 2; i <= rows; i++)
                    {
                        var Bills = new Bills();
                        foreach (var header in headers)
                        {
                            var columLetter = workSheet.GetCoulmnLetter(header);
                            var cellAddress = $"{columLetter}{i}";
                            var cellValue = workSheet.Cell(cellAddress).Value.ToString();
                            var propInfo = Bills.GetType().GetProperties().
                                Where(x => x.GetCustomAttribute<ExcelColumnHeaderAttribute>() != null
                            && x.GetCustomAttribute<ExcelColumnHeaderAttribute>().columnName == header)?.FirstOrDefault() ?? null;
                            if (propInfo != null)
                            {
                                propInfo.SetValue(Bills, cellValue);
                            }
                        }
                        data.Add(Bills);
                        //Debug.WriteLine($"rowNumber {i} inserted");
                    }
                }
            }

            WriteExcel(data);

        }

        public static void WriteExcel(List<Bills> bills)
        {
            var output = @"C:\ddddddddddddddddddd\AggregateBills\Bills\Aggregated.xlsx";
            var headers = Bills.GetExcelColumnHeaders();

            using (var workBook = new XLWorkbook(output))
            {
                var isThere = workBook.TryGetWorksheet("Sheet1", out IXLWorksheet ws);
                var HeaderRow = ws.Row(1);
                HeaderRow.Cell("A").Value = "CUSTOMER CODE";
                HeaderRow.Cell("B").Value = "BUYER CODE";
                HeaderRow.Cell("C").Value = "AMOUNT";
                HeaderRow.Cell("D").Value = "DATE";
                HeaderRow.Cell("E").Value = "UTR";
                HeaderRow.Cell("F").Value = "REMITTER IFSCCODE";
                HeaderRow.Cell("G").Value = "CUSTOMER ACCOUNT NUMBER";
                HeaderRow.Cell("H").Value = "SENDER NAME";
                HeaderRow.Cell("I").Value = "PAYMENT PRODUCT CODE";
                HeaderRow.Cell("J").Value = "BENEFICIARY BANK CODE";
                for (int i = 0; i < bills.Count; i++)
                {
                    var row = ws.Row(i + 2);
                    row.Cell("A").Value = bills[i].CustomerCode;
                    row.Cell("B").Value = bills[i].BuyerCode;
                    row.Cell("C").Value = bills[i].Amount;
                    row.Cell("D").Value = bills[i].Date;
                    row.Cell("E").DataType = XLDataType.Text;
                    row.Cell("E").Value = bills[i].UTR+" "+".";
                    row.Cell("F").Value = bills[i].REMITTERIFSCCODE;
                    row.Cell("G").Value = bills[i].CustomerAccountNumber;
                    row.Cell("H").Value = bills[i].SENDERNAME;
                    row.Cell("I").Value = bills[i].PAYMENTPRODUCTCODE;
                    row.Cell("J").Value = bills[i].BENEFICIARYBANKCODE;
                }
                workBook.SaveAs(output);
            }
        }


    }
}