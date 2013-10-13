using Radvill.Models.AdviseModels;

namespace Radvill.Sockets.Internal.EventData
{
    public class AnswerStartedEvent
    {
        public AnswerStartedEvent(PendingQuestion pendingQuestion)
        {
            ID = pendingQuestion.Question.ID;
        }

        public int ID { get; private set; }
    }
}