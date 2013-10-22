using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radvill.Models.AdviseModels;

namespace Radvill.Sockets.Internal.EventData
{
    public class AnswerSubmittedEvent
    {
        public AnswerSubmittedEvent(Answer answer)
        {
            AnswerID = answer.ID;
            QuestionID = answer.Question.ID;
        }

        public int AnswerID { get; private set; }
        public int QuestionID { get; private set; }
    }
}
