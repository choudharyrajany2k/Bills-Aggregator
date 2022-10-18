using System.Reflection;
using System;
using Spire.Xls;

namespace ChangeExtension
{
    internal class Program
    {
        public static string fileExtension = "*.xls";
        public static string SheetName = "AggregratedData";
        static void Main(string[] args)
        {
            string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
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
            string[] files = Directory.GetFiles(inputFilesPath, fileExtension, SearchOption.TopDirectoryOnly);
            Console.WriteLine("Starting Fetching Data ....");

            foreach (var item in files)
            {
                Console.WriteLine($"ChangingExtension for {item}");
                //var fileInfo = new FileInfo(item);
                Workbook workbook = new Workbook();
                workbook.LoadFromFile($"{item}");
                workbook.SaveToFile($"{item}.xlsx", ExcelVersion.Version2016);
            }
            Console.WriteLine("*********COMPLETED**************");
        }

        public static bool IsPathValid(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            return false;
        }

        //public static string ConvertXLS_XLSX(string file)
        //{
        //    var app = new Microsoft.Office.Interop.Excel.Application();
        //    //var xlsFile = file.FullName;
        //    var xlsFile = file;
        //    var wb = app.Workbooks.Open(xlsFile);
        //    var xlsxFile = xlsFile + "x";
        //    wb.SaveAs(Filename: xlsxFile, FileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
        //    wb.Close();
        //    app.Quit();
        //    return xlsxFile;
        //}
    }
}