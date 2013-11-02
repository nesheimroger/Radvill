using System.ComponentModel.DataAnnotations;

namespace Radvill.WebAPI.Models.Advise
{
    public class PutRequestDTO
    {
        [Required]
        public int RequestID { get; set; }

        [Required]
        public bool StartAnswer { get; set; }
    }
}