using System.ComponentModel.DataAnnotations;

namespace testNetMVC.Models
{
    public class User
    {
        [Key]
        [Display(Name = "Código interno")]
        public int? Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Password { get; set; }

        public override string ToString()
        {
            return Name ?? "";
        }
    }
}