using System.Reflection;

namespace Neft.Aggregation
{
    internal class Bills
    {
        [ExcelColumnHeader("CUSTOMER CODE")]
        public string CustomerCode { get; set; }

        [ExcelColumnHeader("BUYER CODE")]
        public string BuyerCode { get; set; }

        [ExcelColumnHeader("AMOUNT")]
        public string Amount { get; set; }

        [ExcelColumnHeader("DATE")]
        public string Date { get; set; }

        [ExcelColumnHeader("UTR")]
        public string UTR { get; set; }

        [ExcelColumnHeader("REMITTER IFSCCODE")]
        public string REMITTERIFSCCODE { get; set; }

        [ExcelColumnHeader("CUSTOMER ACCOUNT NUMBER")]
        public string CustomerAccountNumber { get; set; }

        [ExcelColumnHeader("SENDER NAME")]
        public string SENDERNAME { get; set; }

        [ExcelColumnHeader("PAYMENT PRODUCT CODE")]
        public string PAYMENTPRODUCTCODE { get; set; }

        [ExcelColumnHeader("BENEFICIARY BANK CODE")]
        public string BENEFICIARYBANKCODE { get; set; }

        public static List<string> GetExcelColumnHeaders()
        {
            var headers = new List<string>();
            var heading = typeof(Bills).GetProperties();
            foreach (var property in heading)
            {
                var header = GetHeader(property);
                if (!String.IsNullOrEmpty(header))
                {
                    headers.Add(header);
                }
            }
            return headers;
        }

        public static string GetHeader(PropertyInfo propInfo)
        {
            var headerAttribute = (ExcelColumnHeaderAttribute)Attribute.GetCustomAttribute(propInfo, typeof(ExcelColumnHeaderAttribute));
            return headerAttribute?.columnName ?? String.Empty;
        }
    }
}

