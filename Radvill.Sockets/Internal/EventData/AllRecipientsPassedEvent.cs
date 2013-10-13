using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radvill.Models.AdviseModels;

namespace Radvill.Sockets.Internal.EventData
{
    public class AllRecipientsPassedEvent
    {
        public AllRecipientsPassedEvent(Question question)
        {
            ID = question.ID;
        }

        public int ID { get; private set; }
    }
}
