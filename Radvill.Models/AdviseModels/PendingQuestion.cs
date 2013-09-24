using System;
using Radvill.Models.UserModels;

namespace Radvill.Models.AdviseModels
{
    public class PendingQuestion
    {
        public int ID { get; set; }
        public bool? Status { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual Question Question { get; set; }
        public virtual Answer Answer { get; set; }
        public virtual User User { get; set; }
    }
}