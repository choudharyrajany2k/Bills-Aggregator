using Microsoft.AspNetCore.Mvc;

namespace VSR.ReceiptGenerator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceiptGeneratorController : ControllerBase
    {
        private readonly ILogger<ReceiptGeneratorController> _logger;
        private readonly IGmailSendMail _gmailsender;


        public ReceiptGeneratorController(ILogger<ReceiptGeneratorController> logger, IGmailSendMail gmailsender)
        {
            _logger = logger;
            _gmailsender = gmailsender;
        }

        [HttpPost(Name = "Send receipt Mail")]
        public IActionResult Get(string to_email, Receipt receipt)
        {
            _gmailsender.MailSendGmail(to_email,receipt.flatNumber, receipt.month, receipt.year, receipt.amount, 
                receipt.dateOfTransaction.ToString(), receipt.transactionId, receipt.modeOfTransaction, receipt.purposeOfTranaction);
            return StatusCode(200);
        }
    }
}