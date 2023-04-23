using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace testNetMVC.Models
{
    public class User
    {
        [Key]
        [Display(Name = "Código interno")]
        public int? Id { get; set; }
        // [Required, EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        // [Required]
        [Display(Name = "Contraseña")]
        public string? Password { get; set; }
        [Display(Name = "Nombre")]
        public string? FirstName { get; set; }
        [Display(Name = "Apellido")]
        public string? LastName { get; set; }
        public string? Avatar { get; set; }
        public int? Role_id { get; set; }

        public Role? Role { get; set; }

        public override string ToString()
        {
            return Email ?? "";
        }
        public static string getHashPassword(string password)
        {
            string salt = "secretito";
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: password,
                        salt: System.Text.Encoding.ASCII.GetBytes(salt),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
        }
    }
}