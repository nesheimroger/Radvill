using System;
using Microsoft.Web.WebSockets;

namespace Radvill.Services.Sockets
{
    public interface ISocketManager
    {
        WebSocketHandler GetWebSocket(string email);
        WebSocketCollection Clients();
        WebSocketHandler GetClient(string email);
    }
}