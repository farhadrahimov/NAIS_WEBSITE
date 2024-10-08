using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NAIS_Website.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        public IActionResult GetAll()
        {
            return View();
        }
    }
}
