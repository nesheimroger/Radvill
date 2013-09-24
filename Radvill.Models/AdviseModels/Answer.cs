using System;
using Radvill.Models.UserModels;

namespace Radvill.Models.AdviseModels
{
    public class Answer
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool? Accepted { get; set; }

        public virtual Question Question { get; set; }
        public virtual User User { get; set; }
    }
}