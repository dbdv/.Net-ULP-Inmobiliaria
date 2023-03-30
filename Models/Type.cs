using System.ComponentModel.DataAnnotations;

namespace testNetMVC.Models
{
    public class Type
    {
        [Key]
        public int? Id { get; set; }
        public string? Label { get; set; }

        public override string ToString()
        {
            return Label;
        }

    }
}