using Microsoft.AspNetCore.Mvc;
using VSR.ReceiptGenerator;

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
        public IActionResult Get(Receipt receipt)
        {
            _gmailsender.MailSendGmail(receipt.flatNumber, receipt.month, receipt.year, receipt.amount, 
                receipt.dateOfTransaction.ToString(), receipt.transactionId, receipt.modeOfTransaction, receipt.purposeOfTranaction);
            return StatusCode(200);
        }
    }
}