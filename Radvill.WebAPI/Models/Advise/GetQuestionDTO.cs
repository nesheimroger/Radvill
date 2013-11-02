using System;
using System.Collections.Generic;

namespace Radvill.WebAPI.Models.Advise
{
    public class GetQuestionDTO
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public int Status { get; set; }
        public IEnumerable<AnswerDTO> Answers { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}