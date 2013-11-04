using System;

namespace Radvill.WebAPI.Models.Advise
{
    public class GetAnswerDTO
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool? Accepted { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}