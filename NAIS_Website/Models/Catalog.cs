namespace NAIS_Website.Models
{
    public class Catalog : BaseModel
    {
        public int Category { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
