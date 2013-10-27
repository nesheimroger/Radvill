using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Radvill.Services.Advisor;
using Radvill.Services.DataFactory;
using Radvill.WebAPI.Models.Advise;

namespace Radvill.WebAPI.Controllers
{
    [Authorize]
    public class RequestController : ApiController
    {
        private readonly IDataFactory _dataFactory;
        private readonly IAdviseManager _adviseManager;

        public RequestController(IDataFactory dataFactory, IAdviseManager adviseManager)
        {
            _dataFactory = dataFactory;
            _adviseManager = adviseManager;
        }

        public HttpResponseMessage Get()
        {
            var user = _dataFactory.UserRepository.GetUserByEmail(User.Identity.Name);
            var currentRequest = _dataFactory.PendingQuestionRepository.GetCurrentByUser(user.ID);
            if (currentRequest != null)
            {
                var requestDto = new GetRequestDTO
                    {
                        ID = currentRequest.ID,
                        Question = currentRequest.Question.Text,
                        StartAnswer = currentRequest.Status,
                        DeadLine = _adviseManager.GetDeadline(currentRequest)
                    };

                Request.CreateResponse(HttpStatusCode.OK, requestDto);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage Put([FromBody]PutRequestDTO respond)
        {
            var pending = _dataFactory.PendingQuestionRepository.GetByID(respond.RequestID);
            if (ModelState.IsValid && pending.User.Email == User.Identity.Name)
            {
                if (respond.StartAnswer)
                {
                    if (_adviseManager.StartAnswer(pending))
                    {
                        var deadline = _adviseManager.GetDeadline(pending);
                        return Request.CreateResponse(HttpStatusCode.OK, deadline);
                    }
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
                _adviseManager.PassQuestion(pending);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
