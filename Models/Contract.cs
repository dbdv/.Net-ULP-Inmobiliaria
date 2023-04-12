using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testNetMVC.Models
{
    public class Contract
    {
        [Key]
        [Display(Name = "Cód. int.")]
        public int? Id { get; set; }
        [Display(Name = "Inicia")]
        public DateTime? From { get; set; }
        [Display(Name = "Finaliza")]
        public DateTime? Until { get; set; }
        [Display(Name = "Valor actual")]
        public Decimal? Fee { get; set; }
        [Display(Name = "Propiedad")]
        public int? Property_id { get; set; }
        [ForeignKey(nameof(Property_id))]
        public Property? Property { get; set; }
        [Display(Name = "Inquilino")]
        public int? Renter_id { get; set; }
        [ForeignKey(nameof(Renter_id))]
        public Renter? Renter { get; set; }

        public string getFrom()
        {
            var separated = From.ToString().Split(" ")[0].Split("/");
            return $"{separated[1]}/{separated[0]}/{separated[2]}";
        }

        public string getUntil()
        {
            var separated = Until.ToString().Split(" ")[0].Split("/");
            return $"{separated[1]}/{separated[0]}/{separated[2]}";
        }

    }
}