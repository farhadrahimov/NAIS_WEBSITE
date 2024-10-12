using System.ComponentModel.DataAnnotations;

namespace NAIS_Website.Models
{
    public class BaseModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "Maksimum icazə verilən simvol sayı: 200")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250, ErrorMessage = "Maksimum icazə verilən simvol sayı: 250")]
        public string? Note { get; set; } = string.Empty;
        public bool DeleteStatus { get; set; } = false;
    }
}
