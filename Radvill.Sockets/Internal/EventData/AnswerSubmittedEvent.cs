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
            ID = answer.Question.ID;
        }

        public int ID { get; private set; }
    }
}
