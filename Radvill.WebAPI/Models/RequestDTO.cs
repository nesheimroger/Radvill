using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Radvill.WebAPI.Models
{
    public class RequestDTO
    {
        [Required]
        public int CategoryID { get; set; }

        [Required]
        public string Question { get; set; }
    }
}