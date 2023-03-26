using Microsoft.AspNetCore.Mvc;
using VSR.ReceiptGenerator;

namespace VSR.ReceiptGenerator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IGmailSendMail _gmailsender;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IGmailSendMail gmailsender)
        {
            _logger = logger;
            _gmailsender = gmailsender;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get(string month, string year)
        {
            _gmailsender.MailSendGmail(month, year);
            return StatusCode(200);
        }
    }
}