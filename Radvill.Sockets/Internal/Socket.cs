using System.Linq;
using Microsoft.Web.WebSockets;

namespace Radvill.Sockets.Internal
{
    public static class Socket
    {
        public static WebSocketCollection Clients;
        
        static Socket()
        {
             Clients = new WebSocketCollection();
        }

        public static WebSocketHandler GetClient(string email)
        {
            return Clients.FirstOrDefault(x => x.WebSocketContext.User.Identity.Name == email);
        }
    }
}