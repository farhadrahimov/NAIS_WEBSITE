namespace NAIS_Website.Models
{
    public class Catalog
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public bool DeleteStatus { get; set; }
    }
}
