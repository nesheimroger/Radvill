using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Web.WebSockets;
using Radvill.Services.Advisor;
using Radvill.Services.DataFactory;
using Radvill.Services.Sockets;

namespace Radvill.Sockets.Internal
{
    public class EventWebSocket : WebSocketHandler
    {
        private readonly IDataFactory _dataFactory;
        private readonly string _email;

        public EventWebSocket(IDataFactory dataFactory, string email)
        {
            _dataFactory = dataFactory;
            _email = email;
            
        }

        public override void OnOpen()
        {
            Socket.Clients.Add(this);
            SetConnectionStatus(true);
        }

        public override void OnClose()
        {
            Socket.Clients.Remove(this);
            SetConnectionStatus(false);


            var adviseManager = DependencyResolver.Current.GetService<IAdviseManager>();
            adviseManager.PassQuestionForUser(_email);
        }


        private void SetConnectionStatus(bool connected)
        {
            var user = _dataFactory.UserRepository.GetUserByEmail(_email);
            user.Connected = connected;
            _dataFactory.UserRepository.Update(user);
            _dataFactory.Commit();
        }

       
        

        
    }
}