using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.Web.WebSockets;
using Radvill.Services.Advisor;
using Radvill.Services.DataFactory;
using Radvill.Services.Sockets;

namespace Radvill.WebAPI.Controllers
{
    [Authorize]
    public class SocketController : ApiController
    {

        private readonly ISocketManager _socketManager;
        private readonly IAdviseManager _adviseManager;

        public SocketController(ISocketManager socketManager, IAdviseManager adviseManager)
        {
            _socketManager = socketManager;
            _adviseManager = adviseManager;
        }

        public HttpResponseMessage Get()
        {
            HttpContext.Current.AcceptWebSocketRequest(_socketManager.GetWebSocket(User.Identity.Name));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        /// <summary>
        /// Should be called when the socket closes, so that we can pass eventual unanswered requests the user have
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Delete()
        {
            _adviseManager.PassQuestionForUser(User.Identity.Name);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }

}
