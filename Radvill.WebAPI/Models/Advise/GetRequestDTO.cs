using System;

namespace Radvill.WebAPI.Models.Advise
{
    public class GetRequestDTO
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public string Category { get; set; }
        public DateTime Deadline { get; set; }
        public bool? StartAnswer { get; set; }
    }
}