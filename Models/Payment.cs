using System.ComponentModel.DataAnnotations;

namespace testNetMVC.Models
{
    public class Payment
    {
        [Key]
        [Display(Name = "Cód. Int.")]
        public int? Id { get; set; }
        [Required]
        [Display(Name = "Pago N°")]
        public int? Number { get; set; }
        [Required]
        [Display(Name = "Fecha")]
        public DateTime? Date { get; set; }
        [Required]
        [Display(Name = "Monto")]
        public Decimal? Amount { get; set; }
        [Display(Name = "Contrato")]
        public int? Contract_id { get; set; }
        public Contract? Contract { get; set; }

        public string getDate()
        {
            var separated = Date.ToString().Split(" ")[0].Split("/");
            return $"{separated[1]}/{separated[0]}/{separated[2]}";
        }

    }
}