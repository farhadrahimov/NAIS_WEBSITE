using Microsoft.AspNetCore.Mvc;
using NAIS_Website.Models;
using NAIS_Website.Services;
using System.Diagnostics;

namespace NAIS_Website.Controllers
{
    public class FileModel
    {
        public string FileName { get; set; } = string.Empty;
        public string FileNameWithoutExtension { get; set; } = string.Empty;
    }

    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;

        public HomeController(IWebHostEnvironment webHostEnvironment, IEmailService emailService)
        {
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
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

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(EmailModel emailModel)
        {
            if (!ModelState.IsValid)
            {
                // Если данные формы не валидны, вернем представление с ошибками
                return View(emailModel);
            }

            bool isEmailSent = await _emailService.SendEmail(emailModel);

            if (isEmailSent)
            {
                // Сообщение об успешной отправке
                ViewBag.Message = "Your message has been sent successfully!";
            }
            else
            {
                // Сообщение об ошибке
                ViewBag.Message = "There was an error sending your message. Please try again later.";
            }

            return View();
        }

        public IActionResult Services()
        {
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "files/photos/services");
            var imageData = Directory.GetFiles(folderPath)
                                .Select(file => new FileModel
                                {
                                    FileName = Path.GetFileName(file),
                                    FileNameWithoutExtension = Path.GetFileNameWithoutExtension(file)
                                })
                                .ToList();
            ViewBag.ImageData = imageData;
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