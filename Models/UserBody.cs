namespace testNetMVC.Models
{
    public class UserBody
    {
        public int? Id { get; set; }
        public int? Role_id { get; set; }
        public string? NewPassword { get; set; }
        public string? CurrentPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IFormFile? Avatar { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

}