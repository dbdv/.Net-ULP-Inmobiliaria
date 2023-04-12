using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testNetMVC.Models
{
    public class Property
    {
        [Key]
        [Display(Name = "Código interno")]
        public int? Id { get; set; }
        [Display(Name = "Porpietario")]
        public int? Owner_id { get; set; }
        [Display(Name = "Uso")]
        public int? Purpose_id { get; set; }
        public Purpose? Purpose { get; set; }
        [Display(Name = "Tipo")]
        public int? Type_id { get; set; }
        public PropType? PropType { get; set; }
        [Required]
        [Display(Name = "N° ambientes")]
        public int? Rooms { get; set; }
        [Display(Name = "Latitud")]
        public Double? Latitude { get; set; }
        [Display(Name = "Longitud")]
        public Double? Longitude { get; set; }
        [Display(Name = "Valor")]
        public Double? Price { get; set; }
        [Required]
        [Display(Name = "Dirreción")]
        public string? Address { get; set; }
        [ForeignKey(nameof(Owner_id))]
        public Owner? Owner { get; set; }

        public override string ToString()
        {
            return $"{Address} | Tipo: {PropType.Label} | Dueño: {Owner.ToString()} | Hab: {Rooms}";
        }

    }
}