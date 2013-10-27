using System;

namespace Radvill.WebAPI.Models.Advise
{
    public class GetRequestDTO
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public DateTime DeadLine { get; set; }
        public bool? StartAnswer { get; set; }
    }
}