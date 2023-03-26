namespace VSR.ReceiptGenerator
{
    public interface IGmailSendMail
    {
        public void MailSendGmail(string month, string year);
    }
}