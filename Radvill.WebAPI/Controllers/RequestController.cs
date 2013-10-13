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
    [Authorize]
    public class RequestController : ApiController
    {
        private readonly IAdviseManager _adviseManager;
        private readonly IDataFactory _dataFactory;

        public RequestController(IAdviseManager adviseManager, IDataFactory dataFactory)
        {
            _adviseManager = adviseManager;
            _dataFactory = dataFactory;
        }

        /// <summary>
        /// Submits a question
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody] NewRequestDTO requestDto)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.Identity.Name;
                var userId = _dataFactory.UserRepository.GetUserByEmail(userEmail).ID;

                return _adviseManager.SubmitQuestion(userId, requestDto.CategoryID, requestDto.Question) 
                    ? Request.CreateResponse(HttpStatusCode.Created, true) 
                    : Request.CreateResponse(HttpStatusCode.OK, false);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);

        }

        /// <summary>
        /// Get all questions and answers for the logged in user
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get(int? id)
        {

            if (ModelState.IsValid)
            {

                if (id.HasValue)
                {
                    var pending = _dataFactory.PendingQuestionRepository.GetByID(id.Value);
                    if (pending != null && pending.User.Email == User.Identity.Name)
                    {
                        var dto = new RequestDTO
                        {
                            ID = pending.ID,
                            Category = pending.Question.Category.Name,
                            IsQuestion = null,
                            Question = pending.Question.Text,
                            TimeStamp = pending.Status == false
                                            ? pending.TimeStamp
                                            : _adviseManager.GetDeadline(pending),
                            Status = pending.Status
                        };
                        return Request.CreateResponse(HttpStatusCode.OK, dto);
                    }
                    
                }

                var user = _dataFactory.UserRepository.GetUserByEmail(User.Identity.Name);

                var requestList = user.Questions.Select(x => new RequestDTO
                {
                    ID = x.ID,
                    Category = x.Category.Name,
                    Status = GetStatusForQuestion(x.ID),
                    Question = x.Text,
                    TimeStamp = x.TimeStamp,
                    IsQuestion = true
                }).ToList();

                requestList.AddRange(user.Answers.Select(x => new RequestDTO
                {
                    ID = x.ID,
                    Status = x.Accepted,
                    Category = x.Question.Category.Name,
                    IsQuestion = false,
                    TimeStamp = x.TimeStamp
                }));

                requestList.AddRange(user.PendingQuestions.Where(x => x.Status == null).Select(x => new RequestDTO
                {
                    ID = x.ID,
                    Category = x.Question.Category.Name,
                    IsQuestion = null,
                    Status = null,
                    Question = x.Question.Text,
                    TimeStamp = x.Question.TimeStamp
                }));


                return Request.CreateResponse(HttpStatusCode.OK, requestList.OrderByDescending(x => x.TimeStamp));

                
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        public HttpResponseMessage Put([FromBody]RespondDTO respond)
        {
            var pending = _dataFactory.PendingQuestionRepository.GetByID(respond.ID);
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

        private bool? GetStatusForQuestion(int id)
        {
            var answers = _dataFactory.AnswerRepository.GetByQuestionId(id);
            if (answers.Any(x => x.Accepted != false))
            {
                return true;
            }
            var pendingQuestions = _dataFactory.PendingQuestionRepository.GetByQuestionID(id);
            return pendingQuestions.Any(x => x.Status == true) ? false : (bool?)null;
        }
    }
}
