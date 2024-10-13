using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NAIS_Website.Database;
using NAIS_Website.Models;
using NAIS_Website.Services;

namespace NAIS_Website.Controllers
{
    [Authorize]
    public class CatalogCategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CatalogCategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await _dbContext.CatalogCategory
                    .Where(c=> !c.DeleteStatus)
                    .OrderByDescending(c=>c.Id)
                    .ToListAsync();

                return View(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CatalogCategory catalogCategory)
        {
            if (ModelState.IsValid)
            {
                if (!CatalogCategoryExists(catalogCategory.Name))
                {
                    _dbContext.CatalogCategory.Add(catalogCategory);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "Bu cür adla Kateqoriya artıq mövcuddur.";
                }
            }
            else
            {
                return View(catalogCategory);
            }
            return View(catalogCategory);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var catalogCategory = await _dbContext.CatalogCategory.FindAsync(id);
            if (catalogCategory == null)
            {
                return NotFound();
            }

            return View(catalogCategory);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CatalogCategory catalogCategory)
        {
            if (id != catalogCategory.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(CatalogCategoryNameExists(catalogCategory.Name, id))
                    {
                        ViewBag.Error = "Bu cür ad digər Kateqoriyada mövcuddur.";
                        return View(catalogCategory);
                    }
                    else
                    {
                        _dbContext.Update(catalogCategory);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatalogCategoryExists(catalogCategory.Name))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(catalogCategory);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsCategoryUsedInCatalog(id))
            {
                var catalogCategory = await _dbContext.CatalogCategory.FindAsync(id);
                if (catalogCategory != null)
                {
                    catalogCategory.DeleteStatus = true;
                    _dbContext.Update(catalogCategory);
                    await _dbContext.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = "Silinə bilməz. Bu Kateqoriya, Kataloqlarda istifadə olunur. Silmək üçün ilk növbədə bu Kateqoriyada olan Kataloqları silməlisiniz.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool CatalogCategoryExists(string name)
        {
            return _dbContext.CatalogCategory
                .Where(e => !e.DeleteStatus)
                .Any(e => e.Name == name);
        }

        private bool CatalogCategoryNameExists(string name, int id)
        {
            return _dbContext.CatalogCategory
                .Where(e => !e.DeleteStatus)
                .Any(e => e.Name == name && e.Id != id);
        }

        private bool IsCategoryUsedInCatalog(int categoryId)
        {
            return _dbContext.Catalog
                .Where(e => !e.DeleteStatus)
                .Any(e => e.Category == categoryId);
        }
    }
}
