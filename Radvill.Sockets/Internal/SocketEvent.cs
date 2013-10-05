using Newtonsoft.Json;
using Radvill.Models.AdviseModels;
using Radvill.Sockets.Internal.Entities;

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



        

    }
}