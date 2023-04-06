using System.ComponentModel.DataAnnotations;

namespace testNetMVC.Models
{
    public class User
    {
        [Key]
        [Display(Name = "Código interno")]
        public int? Id { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public int? RoleId { get; set; }

        public Role? Role { get; set; }

        public override string ToString()
        {
            return Email ?? "";
        }
    }
}