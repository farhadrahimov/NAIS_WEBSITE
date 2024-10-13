namespace NAIS_Website.Models
{
    public class CatalogViewModel : BaseModel
    {
        public int Category { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
    }
}
