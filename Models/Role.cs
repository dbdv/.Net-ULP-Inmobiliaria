using System.ComponentModel.DataAnnotations;

namespace testNetMVC.Models
{
    public class Role
    {
        private IDictionary<string, string> rolesTranslations = new Dictionary<string, string>{
            {"admin", "Administrador"},
            {"employee", "Empleado"},
        };
        [Key]
        [Display(Name = "Código interno")]
        public int? Id { get; set; }
        [Required, EmailAddress]
        public string? Label { get; set; }

        public string getRole()
        {
            return rolesTranslations[Label];
        }
    }
}