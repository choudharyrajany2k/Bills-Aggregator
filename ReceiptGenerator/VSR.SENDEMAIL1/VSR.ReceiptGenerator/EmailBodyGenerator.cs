namespace VSR.ReceiptGenerator
{
    public class EmailBodyGenerator
    {
        public static string GetEmailBody()
        {
            string htmlContent = "";
            var lines = File.ReadAllLines(@"Receipt.html");
            foreach (var item in lines)
            {
                htmlContent += item;
            }
            return htmlContent;
        }
    }
}
