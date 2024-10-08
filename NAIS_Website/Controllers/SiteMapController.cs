using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Linq;

namespace NAIS_Website.Controllers
{
    public class SiteMapController : Controller
    {
        public IActionResult Index()
        {
            // Создаем XML-документ
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var sitemap = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(ns + "urlset",
                    new XElement(ns + "url",
                        new XElement(ns + "loc", $"{Request.Scheme}://{Request.Host}/"),
                        new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", "daily"),
                        new XElement(ns + "priority", "1.0")
                    ),
                    // Добавьте другие URL-адреса сайта
                    new XElement(ns + "url",
                        new XElement(ns + "loc", $"{Request.Scheme}://{Request.Host}/about-us"),
                        new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", "weekly"),
                        new XElement(ns + "priority", "0.8")
                    )
                )
            );

            // Возвращаем XML
            return Content(sitemap.ToString(), "application/xml", Encoding.UTF8);
        }
    }
}
