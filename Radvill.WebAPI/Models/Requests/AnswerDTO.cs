using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Radvill.WebAPI.Models.Requests
{
    public class AnswerDTO
    {
        [Required]
        public int PendingRequestID { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}