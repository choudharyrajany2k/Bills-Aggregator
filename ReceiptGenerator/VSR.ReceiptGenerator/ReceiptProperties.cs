namespace VSR.ReceiptGenerator
{
    public class Receipt
    {
        public string flatNumber { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string amount { get; set; }
        public DateTime dateOfTransaction { get; set; }
        public string transactionId { get; set; }
        public ModeOfTransaction modeOfTransaction = ModeOfTransaction.Online;
        public PurposeOfTransaction purposeOfTranaction = PurposeOfTransaction.Maintenance;
    }
}
