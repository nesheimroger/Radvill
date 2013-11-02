using System.ComponentModel.DataAnnotations;

namespace Radvill.WebAPI.Models.Advise
{
    public class SubmitAnswerDTO
    {
        [Required]
        public int RequestID { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}