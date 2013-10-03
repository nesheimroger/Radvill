using System;

namespace Radvill.WebAPI.Models.Requests
{
    public class RequestDTO
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public bool? Status { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool? IsQuestion { get; set; }
    }
}