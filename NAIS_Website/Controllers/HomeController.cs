using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NAIS_Website.Data;
using NAIS_Website.Database;
using NAIS_Website.Models;
using NAIS_Website.Services;
using System.Diagnostics;
using System.Text.Json;

namespace NAIS_Website.Controllers
{
    public class FileModel
    {
        public string FileName { get; set; } = string.Empty;
        public string FileNameWithoutExtension { get; set; } = string.Empty;
    }

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _memoryCache;
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

        public IActionResult Offering()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Offering(EmailModel emailModel)
        {
            if (!ModelState.IsValid)
            {
                return View(emailModel);
            }

            bool isEmailSent = await _emailService.SendEmail(emailModel);

            if (isEmailSent)
            {
                ViewBag.Message = "Uğurla göndərildi!";
            }
            else
            {
                ViewBag.Message = "Xəta baş verdi. Yenidən cəhd edin";
            }

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Services()
        {
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "files/photos/services");
            var filePath = Path.Combine(folderPath, "services.json");

            var serviceData = new List<ServicesJsonModel>();

            if (System.IO.File.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    serviceData = JsonSerializer.Deserialize<List<ServicesJsonModel>>(stream);
                }
            }
            else
            {
                ViewBag.Error = "Error";
            }
            return View(serviceData);
        }

        public IActionResult Calculator()
        {
            var products = Products.GetProducts();
            return View(products);
        }

        [HttpPost]
        public IActionResult Calculator(string product, string size, int quantity)
        {
            var products = Products.GetProducts();
            var selectedProduct = products.FirstOrDefault(p => p.Name == product);

            if (selectedProduct != null && selectedProduct.Prices.ContainsKey((size, quantity)))
            {
                decimal price = selectedProduct.Prices[(size, quantity)];
                ViewBag.Price = price;
            }
            else
            {
                ViewBag.Price = "-";
            }

            return View(products);
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
            try
            {
                ViewBag.Catalogs = await GetUniqueCatalog();

                if (ViewBag.Catalogs == null)
                {
                    ViewBag.Error = "Kataloq məlumatları tapılmadı";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Xəta baş verdi: {ex.Message}";
            }
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

        public async Task<IActionResult> Details(int categoryId, string CategoryName)
        {
            try
            {
                if (categoryId == 0)
                {
                    ViewBag.Error = "Məlumat tapılmadı";
                    return View();
                }

                var catalogs = await GetCatalogByCategory(categoryId);

                if (catalogs == null)
                {
                    ViewBag.Error = "Məlumat tapılmadı";
                    return View();
                }

                ViewBag.Catalogs = catalogs;
                ViewBag.CategoryName = CategoryName;
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Xəta baş verdi: {ex.Message}";
            }
            return View();
        }

        #region Functions
        [HttpGet]
        public JsonResult GetSizes(string product)
        {
            var products = Products.GetProducts();
            var selectedProduct = products.FirstOrDefault(p => p.Name == product);

            if (selectedProduct != null)
            {
                return Json(selectedProduct.Sizes);
            }

            return Json(new List<string>());
        }
        [HttpGet]
        public JsonResult GetQuantities(string product, string size)
        {
            var products = Products.GetProducts();
            var selectedProduct = products.FirstOrDefault(p => p.Name == product);

            if (selectedProduct != null)
            {
                var quantities = selectedProduct.Prices
                    .Where(p => p.Key.Item1 == size)
                    .Select(p => p.Key.Item2)
                    .ToList();

                if (quantities.Any())
                {
                    return Json(quantities);
                }
            }

            return Json(new List<int>());
        }
        public async Task<List<CatalogViewModel>> GetUniqueCatalog()
        {
            try
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

                return cachedCatalog ?? new List<CatalogViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching catalog: {ex.Message}");
                return new List<CatalogViewModel>();
            }
        }
        public async Task<List<Catalog>> GetCatalogByCategory(int categoryId)
        {
            try
            {
                if (categoryId != 0)
                {
                    if (!_memoryCache.TryGetValue($"CatalogByCategory_{categoryId}", out List<Catalog> cachedCatalog))
                    {
                        var data = _dbContext.Catalog.Where(x => x.Category == categoryId);
                        cachedCatalog = await data.ToListAsync();

                        _memoryCache.Set($"CatalogByCategory_{categoryId}", cachedCatalog, CacheDuration);
                    }
                    return cachedCatalog ?? new List<Catalog>();
                }
                else
                {
                    return new List<Catalog>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching catalog: {ex.Message}");
                return new List<Catalog>();
            }
        }
        #endregion
    }
}