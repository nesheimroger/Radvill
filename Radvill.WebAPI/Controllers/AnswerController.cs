using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Radvill.Services.Advisor;
using Radvill.Services.DataFactory;
using Radvill.WebAPI.Models;
using Radvill.WebAPI.Models.Requests;

namespace Radvill.WebAPI.Controllers
{
    public class AnswerController : ApiController
    {
        private readonly IAdviseManager _adviseManager;
        private readonly IDataFactory _dataFactory;

        public AnswerController(IAdviseManager adviseManager, IDataFactory dataFactory)
        {
            _adviseManager = adviseManager;
            _dataFactory = dataFactory;
        }


        public HttpResponseMessage Post([FromBody] AnswerDTO AnswerDto)
        {
            if (ModelState.IsValid)
            {
                var pending = _dataFactory.PendingQuestionRepository.GetByID(AnswerDto.PendingRequestID);
                if (pending.User.Email == User.Identity.Name)
                {
                    if (_adviseManager.SubmitAnswer(pending, AnswerDto.Answer))
                    {
                        return Request.CreateResponse(HttpStatusCode.Created);
                    }
                    
                }
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);

        }

        public HttpResponseMessage Put([FromBody] EvaluationDTO evaluationDto)
        {
            if (ModelState.IsValid)
            {
                var answer = _dataFactory.AnswerRepository.GetByID(evaluationDto.ID);
                if (answer.Question.User.Email == User.Identity.Name)
                {
                    if (evaluationDto.Accepted)
                    {
                        _adviseManager.AcceptAnswer(answer);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    var passedOn = _adviseManager.DeclineAnswer(answer);
                    return Request.CreateResponse(HttpStatusCode.OK, passedOn);
                }
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
