using System.ComponentModel.DataAnnotations;

namespace NAIS_Website.Models
{
    public class Catalog : BaseModel
    {
        [Required(ErrorMessage = "Bu xananı boş qoymaq olmaz")]
        public int Category { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
