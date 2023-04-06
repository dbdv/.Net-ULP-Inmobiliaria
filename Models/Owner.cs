using System.ComponentModel.DataAnnotations;

namespace testNetMVC.Models
{
    public class Owner
    {
        [Key]
        [Display(Name = "Código interno")]
        public int? Id { get; set; }
        [Required]
        public string? First_name { get; set; }
        [Required]
        public string? Last_name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Dni { get; set; }
        [Required]
        public string? Phone { get; set; }

        public override string ToString()
        {
            return $"{First_name} {Last_name}";
        }

        public string getContactInfo()
        {
            if (Phone != null) return Phone;
            return Email;
        }
    }
}