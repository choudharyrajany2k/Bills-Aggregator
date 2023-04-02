namespace VSR.ReceiptGenerator
{
    public class Receipt
    {
        public string flatNumber { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string amount { get; set; }
        public string dateOfTransaction { get; set; }
        public string transactionId { get; set; } = "XXXXXXXX";
        public ModeOfTransaction modeOfTransaction { get; set; } = ModeOfTransaction.Online;
        public PurposeOfTransaction purposeOfTranaction { get; set; } = PurposeOfTransaction.Maintenance;
    }
}
