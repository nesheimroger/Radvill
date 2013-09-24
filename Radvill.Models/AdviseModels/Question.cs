using System;
using System.Collections.Generic;
using Radvill.Models.UserModels;

namespace Radvill.Models.AdviseModels
{
    public class Question
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}