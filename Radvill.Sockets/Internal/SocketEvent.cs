using Newtonsoft.Json;
using Radvill.Models.AdviseModels;
using Radvill.Sockets.Internal.EventData;

namespace Radvill.Sockets.Internal
{
    public static class SocketEvent
    {

        private const string JsonBody = "[\"{0}\",{1}]";

        public static string QuestionAssigned(PendingQuestion pendingQuestion)
        {
            var eventData = new QuestionAssignedEvent(pendingQuestion);
            var json = string.Format(JsonBody, "QuestionAssigned", JsonConvert.SerializeObject(eventData));
            return json;
        }

        public static string AnswerSubmitted(Answer answer)
        {
            var eventData = new AnswerSubmittedEvent(answer);
            var json = string.Format(JsonBody, "AnswerSubmitted", JsonConvert.SerializeObject(eventData));
            return json;
        }

        public static string AnswerStarted(PendingQuestion pendingQuestion)
        {
            var eventData = new AnswerStartedEvent(pendingQuestion);
            var json = string.Format(JsonBody, "AnswerStarted", JsonConvert.SerializeObject(eventData));
            return json;
        }


        public static string AllRecipientsPassed(Question question)
        {
            var eventData = new AllRecipientsPassedEvent(question);
            var json = string.Format(JsonBody, "AllRecipientsPassed", JsonConvert.SerializeObject(eventData));
            return json;
        }

        public static string AnswerEvaluated(Answer answer)
        {
            var eventData = new AnswerEvaluatedEvent(answer);
            var json = string.Format(JsonBody, "AnswerEvaluated", JsonConvert.SerializeObject(eventData));
            return json;
        }
    }
}