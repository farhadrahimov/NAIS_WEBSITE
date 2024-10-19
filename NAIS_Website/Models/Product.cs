namespace NAIS_Website.Models
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Sizes { get; set; } = new List<string>();
        public Dictionary<(string, int), decimal> Prices { get; set; } = new Dictionary<(string, int), decimal>();
    }
}
