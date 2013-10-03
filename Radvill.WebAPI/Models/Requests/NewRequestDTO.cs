using System.ComponentModel.DataAnnotations;

namespace Radvill.WebAPI.Models.Requests
{
    public class NewRequestDTO
    {
        [Required]
        public int CategoryID { get; set; }

        [Required]
        public string Question { get; set; }
    }
}