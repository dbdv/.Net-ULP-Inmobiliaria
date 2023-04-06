using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testNetMVC.Models
{
    public class Contract
    {
        [Key]
        [Display(Name = "Código interno")]
        public int? Id { get; set; }
        [Display(Name = "Fecha inicio")]
        public DateTime? From { get; set; }
        [Display(Name = "Fecha finalización")]
        public DateTime? Until { get; set; }
        [Display(Name = "Valor")]
        public Decimal? Fee { get; set; }
        [Display(Name = "Propiedad")]
        public int? Property_id { get; set; }
        [ForeignKey(nameof(Property_id))]
        public Property? Property { get; set; }
        [Display(Name = "Inquilino")]
        public int? Renter_id { get; set; }
        [ForeignKey(nameof(Renter_id))]
        public Renter? Renter { get; set; }

    }
}