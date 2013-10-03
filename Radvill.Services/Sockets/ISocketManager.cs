using Microsoft.Web.WebSockets;

namespace Radvill.Services.Sockets
{
    public interface ISocketManager
    {
        WebSocketHandler GetWebSocketHandler(string email);
        WebSocketCollection Clients();
        WebSocketHandler GetClient(string email);
    }
}