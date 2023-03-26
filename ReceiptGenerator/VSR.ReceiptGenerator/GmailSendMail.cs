using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace VSR.ReceiptGenerator
{
    public class GmailSendMail : IGmailSendMail
    {
        //public const string Email = "office.vssafoa@gmail.com";
        public const string Email = "choudharyrajany2k@gmail.com";
        public const string password = "awtgsjjrgprfdahq";
        public void MailSendGmail(string month, string year)
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
                string emailBody = EmailBodyGenerator.GetEmailBody();
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
    }
}
