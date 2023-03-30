using System.ComponentModel.DataAnnotations;

namespace testNetMVC.Models
{
    public class Purpose
    {
        [Key]
        public int? Id { get; set; }
        public string? Description { get; set; }

    }
}