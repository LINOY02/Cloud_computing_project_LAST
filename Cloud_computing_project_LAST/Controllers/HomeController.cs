using Cloud_computing_project_LAST.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace Cloud_computing_project_LAST.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
   
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Reservation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservation(FormCollection form)
        {
            var person = new
            {
                Name = form["name"],
                Tel = form["tel"],
                Seates = form["subject"],
                Date = form["date"],
                Time = form["time"],
                Email = form["2email"]
            };

            if (ModelState.IsValid)
            {
                await SendResevationConfirmationEmail(person.Name!, person.Tel!, person.Seates!, person.Date!, person.Time!, person.Email!);
                return RedirectToAction(nameof(Index));
            }

            return View(person);
        }

        private async Task SendResevationConfirmationEmail(string name, string tel, string seats, string date, string time, string email)
        {
            // Replace these values with your SMTP server details
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "caffena100@gmail.com";
            string smtpPassword = "zybc owcy vprg vmcb";

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                var message = new MailMessage
                {
                    From = new MailAddress("caffena100@gmail.com"),
                    Subject = "Reservation Confirmation",
                    Body = $@"Dear {name},

We are delighted to confirm your reservation at Cafena for the date and time requested. We look forward to hosting you for a memorable dining experience.

Reservation Details:
- Reservation Name: {name}
- Phone Number: {tel}
- Date: {date}
- Time: {time}
- Number of Seats Reserved: {seats}

Should you need to make any changes to your reservation or if you have any special requests, please do not hesitate to contact us at 052-5381648. We will do our best to accommodate your needs.

Please note that your reservation is confirmed for date at time. We kindly ask that you arrive on time to ensure the best possible dining experience.

Thank you for choosing Cafena. We are committed to providing you with excellent service and a delightful culinary journey.

If you have any questions or require further assistance, please feel free to get in touch with us. We look forward to serving you and making your visit exceptional.

Sincerely,

LAST
Meneger
Cafena
052-5381648
caffena100@gmail.com
https://localhost:7227/",
                    IsBodyHtml = false
                };
                message.To.Add(email);
                await client.SendMailAsync(message);
            }
        }


        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contant(FormCollection form)
        {
            var person = new
            {
                Name = form["name"],
                Email = form["email"],
                Seates = form["subject"],
                Massage = form["massage"]
            };

            if (ModelState.IsValid)
            {
                await SendContactConfirmationEmail(person.Name!, person.Email!, person.Seates!, person.Massage! );
                return RedirectToAction(nameof(Index));
            }

            return View(person);
        }

        private async Task SendContactConfirmationEmail(string name, string email, string seats, string message)
        {
            var smtpHost = "smtp.gmail.com";
            var smtpPort = 587;
            var smtpUsername = "caffena100@gmail.com";
            var smtpPassword = "zybc owcy vprg vmcb";
            var smtpRecipient = "your-recipient@example.com";

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(email),
                    Subject = "Contact Form Submission Confirmation",
                    Body = $"Name: {name}\nEmail: {email}\nSeats: {seats}\nMessage: {message}"
                };

                mailMessage.To.Add(smtpRecipient);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }


        public IActionResult Gallery()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult BlogDetails()
        {
            return View();
        }

        public IActionResult Chefs()
        {
            return View();
        }

        public IActionResult Story()
        {
            return View();
        }

        public IActionResult Shop()
        {
            return View();
        }

        public IActionResult Faq()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
 }