using System.ComponentModel.DataAnnotations;

namespace Radvill.WebAPI.Models.Advise
{
    public class SubmitEvaluationDTO
    {
        [Required]
        public int AnswerID { get; set; }

        [Required]
        public bool Accepted { get; set; }
    }
}