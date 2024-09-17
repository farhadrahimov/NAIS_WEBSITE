using Microsoft.AspNetCore.Mvc;
using NAIS_Website.Models;
using System.Diagnostics;

namespace NAIS_Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "files/photos/carousel");
            var imagePaths = Directory.GetFiles(folderPath)
                          .Select(Path.GetFileName)
                          .ToList();
            ViewBag.ImagePaths = imagePaths;
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Calculating()
        {
            return View();
        }

        public IActionResult Partners()
        {
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "files/photos/partners");
            var imagePaths = Directory.GetFiles(folderPath)
                          .Select(Path.GetFileName)
                          .ToList();
            ViewBag.ImagePaths = imagePaths;
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Catalog()
        {
            return View();
        }

        public IActionResult Privacy()
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