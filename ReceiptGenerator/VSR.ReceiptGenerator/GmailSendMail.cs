using System.Globalization;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace VSR.ReceiptGenerator
{
    public enum ModeOfTransaction
    {
        Online,
        Cash
    }

    public enum PurposeOfTransaction
    {
        Maintenance,
        PartyHall
    }

    public class GmailSendMail : IGmailSendMail
    {
        //public const string Email = "office.vssafoa@gmail.com";
        public const string Email = "choudharyrajany2k@gmail.com";
        public const string password = "awtgsjjrgprfdahq";
        public void MailSendGmail(string flatNumber, string month, string year, string amount, string dateOfTransaction, string transactionId,
            ModeOfTransaction modeOfTransaction = ModeOfTransaction.Online,
            PurposeOfTransaction purposeOfTranaction = PurposeOfTransaction.Maintenance)
        {
            try
            {
                SmtpClient smtp = new("smtp.gmail.com")
                {
                    Port = 587, // TLS
                    //Port = 465, // SSl
                    Credentials = new System.Net.NetworkCredential(Email, password),
                    EnableSsl = true,
                };
                MailAddress from = new MailAddress(Email);
                MailAddress to = new MailAddress(Email);
                string emailBody = FillBodyTemplate(flatNumber, month, year, amount, dateOfTransaction, transactionId, modeOfTransaction, purposeOfTranaction);
                MailMessage mailmsg = new()
                {
                    Subject = $"Maintainence Receipt for {month}-{year}",
                    Body = emailBody,
                    From = from,
                    IsBodyHtml = true,
                    //BodyEncoding = System.Text.Encoding.Unicode,
                    HeadersEncoding = System.Text.Encoding.Unicode,
                    SubjectEncoding = System.Text.Encoding.Unicode,
                };
                mailmsg.To.Add(to);
                AlternateView view = ContentToAlternateView(emailBody);
                mailmsg.AlternateViews.Add(view);
                smtp.Send(mailmsg);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static AlternateView ContentToAlternateView(string content)
        {
            var imgCount = 0;
            List<LinkedResource> resourceCollection = new List<LinkedResource>();
            foreach (Match m in Regex.Matches(content, "<img(?<value>.*?)>"))
            {
                imgCount++;
                var imgContent = m.Groups["value"].Value;
                string type = Regex.Match(imgContent, ":(?<type>.*?);base64,").Groups["type"].Value;
                var base64 = Regex.Match(imgContent, "base64,(?<base64>.*?)\"");//.Groups["base64"].Value;
                //Match base64 = Regex.Match(imgContent, "base64,(?<base64>.*?)\");
                var groups1 = base64.Groups["base64"].Value;
                if (String.IsNullOrEmpty(type) || String.IsNullOrEmpty(groups1))
                {
                    //ignore replacement when match normal <img> tag
                    continue;
                }
                var replacement = " src=\"cid:" + imgCount + "\"";
                content = content.Replace(imgContent, replacement);
                var tempResource = new LinkedResource(Base64ToImageStream(groups1), new ContentType(type))
                {
                    ContentId = imgCount.ToString()
                };
                resourceCollection.Add(tempResource);
            }

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);
            foreach (var item in resourceCollection)
            {
                alternateView.LinkedResources.Add(item);
            }

            return alternateView;
        }
        public static Stream Base64ToImageStream(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            return ms;
        }

        private static string FillBodyTemplate(string flatNumber, string month, string year, string amount, string dateOfTransaction, string transactionId,
            ModeOfTransaction modeOfTransaction = ModeOfTransaction.Online,
            PurposeOfTransaction purposeOfTranaction = PurposeOfTransaction.Maintenance)
        {
            string emailBody = EmailBodyGenerator.GetEmailBody();
            var isParsedBody = DateTime.TryParse(dateOfTransaction, out DateTime _dateOfTransaction);
            _dateOfTransaction = isParsedBody ? _dateOfTransaction : DateTime.MinValue;
            var isParsedNo = DateTime.TryParseExact(dateOfTransaction, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _dateOfTransactionNo);
            _dateOfTransactionNo = isParsedNo ? _dateOfTransactionNo : DateTime.MinValue;
            var amountInWords = NumberToWord.ConvertWholeNumber(amount);
            var receipt_isssue_date = DateTime.Now.Date.ToString();
            string filledEmailBody = emailBody.Replace("{flatNumber}", flatNumber)
                .Replace("{date_of_transaction_MMDDYYYY}", _dateOfTransactionNo.ToString("ddMMyyyy"))
                .Replace("{Purpose}", purposeOfTranaction.ToString())
                .Replace("{mode_of_transaction}", modeOfTransaction.ToString())
                .Replace("{transactionId}", transactionId ?? "XXXXXXXXXX")
                .Replace("{date_of_transaction_MM-DD-YYYY}", _dateOfTransaction.ToString("dd-MM-yyyy"))
                .Replace("{amount_in_words}", amountInWords)
                .Replace("{month}", month)
                .Replace("{year}", year)
                .Replace("{amount}", amount)
                .Replace("{Receipt_Isssue_Date}", receipt_isssue_date);
            return filledEmailBody;
        }
    }
}
