
using ClosedXML.Excel;
using Extract.Excel.Loader.Extensions;
using System.Reflection;

namespace Neft.Aggregation
{
    public class Program
    {
        public static string fileExtension = "*.xlsx";
        public static string SheetName = "AggregratedData";
        static void Main(string[] args)
        {
            string AssemblyName = Assembly.GetExecutingAssembly()?.GetName()?.Name ?? string.Empty;
            if (string.IsNullOrEmpty(AssemblyName))
            {
                Console.Error.WriteLine("Assembly name not found");
                return;
            }
            if (args.Length < 1)
            {
                Console.Error.WriteLine("************************* Excel Locations Missing From Argument********************************");
                Console.Error.WriteLine("************************* Example *************************************************************");
                Console.Error.WriteLine($"{AssemblyName} {"<FILE PATH Containing Bills documents>"}");
                return;
            }
            var inputFilesPath = args[0];
            if (inputFilesPath == null || !IsPathValid(inputFilesPath))
            {
                Console.Error.WriteLine($"{inputFilesPath} doesn't exists !");
                return;
            }
            var outPath = @$"{args[0]}\output\AggregratedData_{DateTime.UtcNow.Ticks}.xlsx";
            Console.WriteLine($"Reading xlsx Files from {inputFilesPath}");
            string[] files = Directory.GetFiles(inputFilesPath, fileExtension, SearchOption.TopDirectoryOnly);
            var data = new List<Bills>();
            var headers = Bills.GetExcelColumnHeaders();
            Console.WriteLine("Starting Fetching Data ....");

            foreach (var item in files)
            {
                Console.WriteLine($"Fetching Data From {item}");
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
                                Where(x => x?.GetCustomAttribute<ExcelColumnHeaderAttribute>() != null
                            && x.GetCustomAttribute<ExcelColumnHeaderAttribute>()?.columnName == header)?.FirstOrDefault() ?? null;
                            if (propInfo != null)
                            {
                                propInfo.SetValue(Bills, cellValue);
                            }
                        }
                        data.Add(Bills);
                    }
                }
            }

            WriteExcel(data, outPath);
            Console.WriteLine($"Output will ve aggregated in file {outPath}");
            Console.WriteLine($"****************************COMPLETED**********************************");
        }

        public static bool IsPathValid(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            return false;
        }

        public static bool IsCorrectFileExtension(string path)
        {
            if (Path.GetExtension(path) == fileExtension)
            {
                return true;
            }
            return false;
        }

        public static FileStream CreateIfNotExisting(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File Created {filePath}");
                using var fs = File.Create(filePath);
                return fs;
            }
            return null;
        }

        static void WriteExcel(List<Bills> bills, string outputPath)
        {
            if (!string.IsNullOrEmpty(outputPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            }
            else
            {
                Console.WriteLine("OutputPath Can't be null");
                return;
            }
            using (var fs = File.Create(outputPath,1024,FileOptions.None))
            {
                using (var workBook = new XLWorkbook())
                {
                    var ws = workBook.AddWorksheet(SheetName);
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
                        row.Cell("E").Value = bills[i].UTR + " " + ".";
                        row.Cell("F").Value = bills[i].REMITTERIFSCCODE;
                        row.Cell("G").Value = bills[i].CustomerAccountNumber;
                        row.Cell("H").Value = bills[i].SENDERNAME;
                        row.Cell("I").Value = bills[i].PAYMENTPRODUCTCODE;
                        row.Cell("J").Value = bills[i].BENEFICIARYBANKCODE;
                    }
                    workBook.SaveAs(fs);
                }
            }
        }
    }
}