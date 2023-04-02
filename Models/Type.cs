using System.ComponentModel.DataAnnotations;

namespace testNetMVC.Models
{
    public class PropType
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