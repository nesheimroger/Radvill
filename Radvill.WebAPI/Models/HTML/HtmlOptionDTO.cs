using System.ComponentModel.DataAnnotations;

namespace Radvill.WebAPI.Models.HTML
{
    public class HtmlOptionDTO
    {
        [Required]
        public int Value { get; set; }

        [Required]
        public string Text { get; set; }
    }
}