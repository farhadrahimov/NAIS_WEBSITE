using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NAIS_Website.Database;
using NAIS_Website.Models;
using NAIS_Website.Services;

namespace NAIS_Website.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IImageService _imageService;

        public CatalogController(ApplicationDbContext dbContext, IImageService imageService)
        {
            _dbContext = dbContext;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, int page = 1, int pageSize = 10)
        {
            try
            {
                ViewBag.CurrentSortOrder = sortOrder;

                var query = from catalog in _dbContext.Catalog
                            join catalogCategory in _dbContext.CatalogCategory on catalog.Category equals catalogCategory.Id
                            where !catalog.DeleteStatus
                            orderby catalog.Id descending
                            select new CatalogViewModel
                            {
                                Id = catalog.Id,
                                Name = catalog.Name ?? string.Empty,
                                CategoryName = catalogCategory.Name ?? string.Empty,
                                Note = catalog.Note ?? string.Empty,
                                ImagePath = catalog.ImagePath ?? string.Empty
                            };
                switch (sortOrder)
                {
                    case "dateDesc":
                        query = query.OrderByDescending(x => x.Id);
                        break;
                    case "dateAsc":
                        query = query.OrderBy(x => x.Id);
                        break;
                    case "byCateg":
                        query = query.OrderBy(x => x.CategoryName);
                        break;
                    case "nameAZ":
                        query = query.OrderBy(x => x.Name);
                        break;
                };

                var pagedResult = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                int totalItems = await query.CountAsync();

                var viewModel = new CatalogPagination
                {
                    Catalogs = pagedResult,
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Xəta baş verdi";
                return View(new CatalogViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await CallCatalogCategory();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Catalog catalog, IFormFile imageFile)
        {
            if (!string.IsNullOrWhiteSpace(catalog.Name) && catalog.Category != 0 && imageFile != null)
            {

                catalog.ImagePath = await _imageService.SaveImageAsync(imageFile);
                _dbContext.Catalog.Add(catalog);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("imageFile", "Şəkil yüklə");
            ViewBag.ImageFileError = "Şəkil seçməmisiniz.";
            ViewBag.Categories = await CallCatalogCategory();
            return View(catalog);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _dbContext.Catalog.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Catalog catalog, IFormFile imageFile)
        {
            if (id != catalog.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    if (!string.IsNullOrEmpty(catalog.ImagePath))
                    {
                        _imageService.DeleteImage(catalog.ImagePath);
                    }

                    catalog.ImagePath = await _imageService.SaveImageAsync(imageFile);
                }
                _dbContext.Catalog.Update(catalog);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(catalog);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            var catalog = await _dbContext.Catalog.FindAsync(id);
            if (catalog != null)
            {
                if (!string.IsNullOrEmpty(catalog.ImagePath))
                {
                    _imageService.DeleteImage(catalog.ImagePath);
                }

                catalog.DeleteStatus = true;
                _dbContext.Catalog.Update(catalog);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<CatalogCategory>> CallCatalogCategory()
        {
            return await _dbContext.CatalogCategory.Select(c => new CatalogCategory
            {
                Id = c.Id,
                Name = c.Name,
            }).ToListAsync();
        }
    }
}