using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Radvill.WebAPI.Models
{
    public class EvaluationDTO
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public bool Accepted { get; set; }
    }
}