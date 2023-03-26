using System.Net.Mail;
using VSR.ReceiptGenerator;

namespace VSR.SendEmail
{
    public class GmailSendMail : IGmailSendMail
    {
        //public const string Email = "office.vssafoa@gmail.com";
        public const string Email = "choudharyrajany2k@gmail.com";
        public const string password = "awtgsjjrgprfdahq";
        public void MailSendGmail(string month,string year)
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
                    BodyEncoding=System.Text.Encoding.Unicode,
                    HeadersEncoding=System.Text.Encoding.Unicode,
                    SubjectEncoding=System.Text.Encoding.Unicode,
                };
                mailmsg.To.Add(to);

                smtp.Send(mailmsg);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
