using System;
using System.Collections.ObjectModel;
using Radvill.Models.AdviseModels;

namespace Radvill.Models.UserModels
{
    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Created { get; set; }
        public bool Connected { get; set; }

        public virtual AdvisorProfile AdvisorProfile { get; set; }
        public virtual Collection<Answer> Answers { get; set; } 
        public virtual Collection<Question> Questions { get; set; } 
        public virtual Collection<PendingQuestion> PendingQuestions { get; set; } 

    }
}