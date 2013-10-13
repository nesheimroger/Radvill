using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Radvill.WebAPI.Models.Requests
{
    public class RespondDTO
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public bool StartAnswer { get; set; }
    }
}