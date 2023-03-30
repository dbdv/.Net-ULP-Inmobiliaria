using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testNetMVC.Models
{
    public class Property
    {
        [Key]
        [Display(Name = "Código interno")]
        public int? Id { get; set; }
        public int? Purpose_id { get; set; }
        public int? Owner_id { get; set; }
        public int? Type_id { get; set; }
        [Required]
        public int? rooms { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public float? Price { get; set; }
        [Required]
        public string? Address { get; set; }
        [ForeignKey(nameof(Owner_id))]
        public Owner? owner { get; set; }

    }
}