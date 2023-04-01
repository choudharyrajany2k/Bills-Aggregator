namespace VSR.ReceiptGenerator
{
    public interface IGmailSendMail
    {
        public void MailSendGmail(string flatNumber, string month, string year, string amount, string dateOfTransaction, string transactionId,
            ModeOfTransaction modeOfTransaction = ModeOfTransaction.Online,
            PurposeOfTransaction purposeOfTranaction = PurposeOfTransaction.Maintenance);
    }
}