﻿using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Linq;

namespace NAIS_Website.Controllers
{
    public class SiteMapController : Controller
    {
        public IActionResult Index()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var sitemap = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(ns + "urlset",
                    new XElement(ns + "url",
                        new XElement(ns + "loc", $"{Request.Scheme}://{Request.Host}/"),
                        new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", "daily"),
                        new XElement(ns + "priority", "1.0")
                    ),
                        new XElement(ns + "url",
                        new XElement(ns + "loc", $"{Request.Scheme}://{Request.Host}/home/aboutus"),
                        new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", "weekly"),
                        new XElement(ns + "priority", "0.9")
                    ),
                        new XElement(ns + "url",
                        new XElement(ns + "loc", $"{Request.Scheme}://{Request.Host}/home/services"),
                        new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", "weekly"),
                        new XElement(ns + "priority", "0.8")
                    ),
                        new XElement(ns + "url",
                        new XElement(ns + "loc", $"{Request.Scheme}://{Request.Host}/home/catalog"),
                        new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", "weekly"),
                        new XElement(ns + "priority", "0.8")
                    ),
                        new XElement(ns + "url",
                        new XElement(ns + "loc", $"{Request.Scheme}://{Request.Host}/home/partners"),
                        new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", "weekly"),
                        new XElement(ns + "priority", "0.7")
                    ),
                        new XElement(ns + "url",
                        new XElement(ns + "loc", $"{Request.Scheme}://{Request.Host}/home/calculator"),
                        new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", "weekly"),
                        new XElement(ns + "priority", "0.6")
                    ),
                        new XElement(ns + "url",
                        new XElement(ns + "loc", $"{Request.Scheme}://{Request.Host}/home/offering"),
                        new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", "weekly"),
                        new XElement(ns + "priority", "0.5")
                    ),
                        new XElement(ns + "url",
                        new XElement(ns + "loc", $"{Request.Scheme}://{Request.Host}/home/contact"),
                        new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", "weekly"),
                        new XElement(ns + "priority", "0.8")
                    )
                )
            );

            return Content(sitemap.ToString(), "application/xml", Encoding.UTF8);
        }
    }
}
