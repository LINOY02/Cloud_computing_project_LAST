using Cloud_computing_project_LAST.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cloud_computing_project_LAST.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        public IActionResult Contact()
        {
            return View();
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

        public IActionResult ProductDetails()
        {
            return View();
        }

        public IActionResult Try()
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