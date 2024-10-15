using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NAIS_Website.Database;
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
        private readonly IMemoryCache _memoryCache;
        private readonly ApplicationDbContext _dbContext;
        private readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public HomeController(IWebHostEnvironment webHostEnvironment,
            IEmailService emailService, 
            ApplicationDbContext dBContext, 
            IMemoryCache memoryCache)
        {
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
            _dbContext = dBContext;
            _memoryCache = memoryCache;
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
                return View(emailModel);
            }

            bool isEmailSent = await _emailService.SendEmail(emailModel);

            if (isEmailSent)
            {
                ViewBag.Message = "Your message has been sent successfully!";
            }
            else
            {
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

        public async Task<IActionResult> Catalog()
        {
            ViewBag.Catalogs = await GetUniqueCatalog();
            return View();
        }

        public async Task<List<CatalogViewModel>> GetUniqueCatalog()
        {
            if (!_memoryCache.TryGetValue("UniqueCatalog", out List<CatalogViewModel> cachedCatalog))
            {
                var query = from catalog in _dbContext.Catalog
                            join catalogCategory in _dbContext.CatalogCategory
                            on catalog.Category equals catalogCategory.Id into catalogGroup
                            from catalogCategory in catalogGroup.DefaultIfEmpty()
                            where !catalog.DeleteStatus
                            && catalog.Id == (from c1 in _dbContext.Catalog
                                              where c1.Category == catalog.Category && !c1.DeleteStatus
                                              select c1.Id).Max()
                            orderby catalog.Category
                            select new CatalogViewModel
                            {
                                Id = catalog.Id,
                                Name = catalog.Name ?? string.Empty,
                                Category = catalog.Category,
                                CategoryName = catalogCategory != null ? catalogCategory.Name ?? string.Empty : string.Empty,
                                ImagePath = catalog.ImagePath ?? string.Empty
                            };

                cachedCatalog = await query.ToListAsync();

                _memoryCache.Set("UniqueCatalog", cachedCatalog, CacheDuration);
            }
            return cachedCatalog;
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