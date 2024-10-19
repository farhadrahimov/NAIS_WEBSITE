using Microsoft.AspNetCore.Mvc;
using NAIS_Website.Models;

namespace NAIS_Website.Data
{
    public static class Products
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>
    {
        new Product
        {
            Name = "Kraft Çanta",
            Sizes = new List<string> { "280 x 370 x 150", "250 x 310 x 150" },
            Prices = new Dictionary<(string, int), decimal>
            {
                { ("280 x 370 x 150", 1500), 630 },
                { ("280 x 370 x 150", 3000), 1200 },
                { ("250 x 310 x 150", 1500), 600 },
                { ("250 x 310 x 150", 3000), 1140 }
            }
        },
        new Product
        {
            Name = "Təkli burger qutusu",
            Sizes = new List<string> { "STANDART" },
            Prices = new Dictionary<(string, int), decimal>
            {
                { ("STANDART", 1000), 380 },
                { ("STANDART", 3000), 900 }
            }
        },
        new Product
        {
            Name = "2-li burger qutusu",
            Sizes = new List<string> { "STANDART" },
            Prices = new Dictionary<(string, int), decimal>
            {
                { ("STANDART", 1000), 340 },
                { ("STANDART", 3000), 900 }
            }
        },
        new Product
        {
            Name = "Free qutusu",
            Sizes = new List<string> { "90 qr", "160 qr" },
            Prices = new Dictionary<(string, int), decimal>
            {
                { ("90 qr", 1500), 195 },
                { ("90 qr", 3000), 330 },
                { ("160 qr", 1500), 285 },
                { ("160 qr", 3000), 480 }
            }
        },
        new Product
        {
            Name = "Sandwich qutusu",
            Sizes = new List<string> { "STANDART" },
            Prices = new Dictionary<(string, int), decimal>
            {
                { ("STANDART", 1500), 570 },
                { ("STANDART", 3000), 990 }
            }
        },
        new Product
        {
            Name = "Roll qutusu",
            Sizes = new List<string> { "STANDART" },
            Prices = new Dictionary<(string, int), decimal>
            {
                { ("STANDART", 1500), 525 },
                { ("STANDART", 3000), 900 }
            }
        },
        new Product
        {
            Name = "American Service",
            Sizes = new List<string> { "400 x 300" },
            Prices = new Dictionary<(string, int), decimal>
            {
                { ("400 x 300", 3000), 270 },
                { ("400 x 300", 5000), 300 }
            }
        },
        new Product
        {
            Name = "Yağlı kağız",
            Sizes = new List<string> { "20 x 30", "40 x 30" },
            Prices = new Dictionary<(string, int), decimal>
            {
                { ("20 x 30", 5000), 250 },
                { ("40 x 30", 5000), 550 }
            }
        },
        new Product
        {
            Name = "Çanta 230qr",
            Sizes = new List<string> { "A5", "A4", "A3", "A2" },
            Prices = new Dictionary<(string, int), decimal>
            {
                { ("A5", 100), 200 }, { ("A5", 500), 430 }, { ("A5", 1000), 675 },
                { ("A4", 100), 250 }, { ("A4", 500), 590 }, { ("A4", 1000), 940 },
                { ("A3", 100), 345 }, { ("A3", 500), 940 }, { ("A3", 1000), 1570 },
                { ("A2", 100), 800 }, { ("A2", 500), 2175 }, { ("A2", 1000), 3280 }
            }
        }
    };
        }
    }
}
