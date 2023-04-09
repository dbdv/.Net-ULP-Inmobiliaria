namespace testNetMVC.Models
{
    public class UpdateUserBody
    {
        public int? Id { get; set; }
        public string? NewPassword { get; set; }
        public string? CurrentPassword { get; set; }
        public IFormFile? Avatar { get; set; }
        public string? AvatarUrl { get; set; }
    }

}