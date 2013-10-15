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

        public SocketController(ISocketManager socketManager)
        {
            _socketManager = socketManager;
        }

        public HttpResponseMessage Get()
        {
            HttpContext.Current.AcceptWebSocketRequest(_socketManager.GetWebSocket(User.Identity.Name));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }


    }

}
