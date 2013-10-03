using System;
using Microsoft.Web.WebSockets;
using Radvill.Services.DataFactory;
using Radvill.Services.Sockets;
using Radvill.Sockets.Internal;

namespace Radvill.Sockets.Public
{
    public class SocketManager : ISocketManager
    {

        private readonly IDataFactory _dataFactory;

        public SocketManager(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        public WebSocketHandler GetWebSocketHandler(string email)
        {
            return new EventWebSocketHandler(_dataFactory, email);
        }

        public WebSocketCollection Clients()
        {
            return Socket.Clients;
        }

        public WebSocketHandler GetClient(string email)
        {
            return Socket.GetClient(email);
        }
    }
}
