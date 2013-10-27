using System.ComponentModel.DataAnnotations;

namespace Radvill.WebAPI.Models.Advise
{
    public class SubmitQuestionDTO
    {
        [Required]
        public int CategoryID { get; set; }

        [Required]
        public string Question { get; set; }
    }
}