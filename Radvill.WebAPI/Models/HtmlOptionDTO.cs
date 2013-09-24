﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Radvill.WebAPI.Models
{
    public class HtmlOptionDTO
    {
        [Required]
        public int Value { get; set; }

        [Required]
        public string Text { get; set; }
    }
}