using Radvill.Models.AdviseModels;

namespace Radvill.Sockets.Internal.EventData
{
    public class AnswerEvaluatedEvent
    {
        public AnswerEvaluatedEvent(Answer answer)
        {
            ID = answer.ID;
            Accepted = answer.Accepted;
        }

        public int ID { get; set; }
        public bool? Accepted { get; set; }
    }
}