using System.ComponentModel.DataAnnotations;

namespace testNetMVC.Models
{
    public class Role
    {
        [Key]
        [Display(Name = "Código interno")]
        public int? Id { get; set; }
        [Required, EmailAddress]
        public string? Label { get; set; }
    }
}