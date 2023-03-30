using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testNetMVC.Models
{
    public class Property
    {
        [Key]
        [Display(Name = "Código interno")]
        public int? Id { get; set; }
        public int? Owner_id { get; set; }
        public int? Purpose_id { get; set; }
        public Purpose? Purpose { get; set; }
        public int? Type_id { get; set; }
        public Type? Type { get; set; }
        [Required]
        public int? rooms { get; set; }
        public Double? Latitude { get; set; }
        public Double? Longitude { get; set; }
        public Double? Price { get; set; }
        [Required]
        public string? Address { get; set; }
        [ForeignKey(nameof(Owner_id))]
        public Owner? Owner { get; set; }

    }
}