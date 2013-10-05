using Radvill.Models.AdviseModels;

namespace Radvill.Sockets.Internal.Entities
{
    public class QuestionAssignedEvent
    {
        public QuestionAssignedEvent(PendingQuestion pendingQuestion)
        {
            ID = pendingQuestion.ID;
        }

        public int ID { get; set; }
    }
}