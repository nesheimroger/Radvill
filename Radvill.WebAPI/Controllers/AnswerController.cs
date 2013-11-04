using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Radvill.Services.Advisor;
using Radvill.Services.DataFactory;
using Radvill.WebAPI.Models;
using Radvill.WebAPI.Models.Advise;

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


        public HttpResponseMessage Get()
        {
            var user = _dataFactory.UserRepository.GetUserByEmail(User.Identity.Name);
            var answers = user.Answers.Select(x => new GetAnswerDTO
                {
                    ID = x.ID,
                    Accepted = x.Accepted,
                    Answer = x.Text,
                    Category = x.Question.Category.Name,
                    Question = x.Question.Text,
                    TimeStamp = x.TimeStamp
                }).OrderByDescending(x => x.TimeStamp);

            return Request.CreateResponse(HttpStatusCode.OK, answers);
        }

        public HttpResponseMessage Get(int id)
        {
            if (ModelState.IsValid)
            {
                var answer = _dataFactory.AnswerRepository.GetByID(id);
                if (answer.User.Email == User.Identity.Name)
                {
                    var answerDto = new GetAnswerDTO
                        {
                            ID = answer.ID,
                            Accepted = answer.Accepted,
                            Category = answer.Question.Category.Name,
                            Answer = answer.Text,
                            Question = answer.Question.Text,
                            TimeStamp = answer.TimeStamp
                        };
                    return Request.CreateResponse(HttpStatusCode.OK, answerDto);
                }
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        public HttpResponseMessage Post([FromBody] SubmitAnswerDTO answerDto)
        {
            if (ModelState.IsValid)
            {
                var pending = _dataFactory.PendingQuestionRepository.GetByID(answerDto.RequestID);
                if (pending.User.Email == User.Identity.Name && _adviseManager.SubmitAnswer(pending, answerDto.Answer))
                {
                    return Request.CreateResponse(HttpStatusCode.Created);                 
                }
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        public HttpResponseMessage Put([FromBody] SubmitEvaluationDTO evaluationDto)
        {
            if (ModelState.IsValid)
            {
                var answer = _dataFactory.AnswerRepository.GetByID(evaluationDto.AnswerID);
                if (answer.Question.User.Email == User.Identity.Name)
                {
                    if (evaluationDto.Accepted)
                    {
                        _adviseManager.AcceptAnswer(answer);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    var passedToSomeoneElse = _adviseManager.DeclineAnswer(answer);
                    return Request.CreateResponse(HttpStatusCode.OK, passedToSomeoneElse);
                }
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
