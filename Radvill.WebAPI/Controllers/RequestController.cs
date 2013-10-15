using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Radvill.Models.AdviseModels;
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
        public HttpResponseMessage Get(int? id = null)
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
                            Type = 3,
                            Question = pending.Question.Text,
                            TimeStamp = pending.Status == false
                                            ? pending.TimeStamp
                                            : _adviseManager.GetDeadline(pending),
                            Status = GetStatusForPending(pending)
                        };
                        return Request.CreateResponse(HttpStatusCode.OK, dto);
                    }
                    
                }

                var user = _dataFactory.UserRepository.GetUserByEmail(User.Identity.Name);

                var requestList = user.Questions.Select(x => new RequestDTO
                {
                    ID = x.ID,
                    Category = x.Category.Name,
                    Status = GetStatusForQuestion(x),
                    Question = x.Text,
                    TimeStamp = x.TimeStamp,
                    Type = 1
                }).ToList();

                requestList.AddRange(user.Answers.Select(x => new RequestDTO
                {
                    ID = x.ID,
                    Status = GetStatusForAnswer(x),
                    Category = x.Question.Category.Name,
                    Type = 2,
                    TimeStamp = x.TimeStamp
                }));

                requestList.AddRange(user.PendingQuestions.Where(x => x.Status == null).Select(x => new RequestDTO
                {
                    ID = x.ID,
                    Category = x.Question.Category.Name,
                    Type = 3,
                    Status = GetStatusForPending(x),
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

        private int GetStatusForQuestion(Question question)
        {
            

            if (question.Answers.Any(x => x.Accepted == true))
            {
                return 6;
            }

            if (question.Answers.All(x => x.Accepted == false) && question.Stopped)
            {
                return 5;
            }

            if (question.Answers.Any(x => x.Accepted == false))
            {
                return 4;
            }

            if (question.Answers.Any(x => x.Accepted == null))
            {
                return 3;
            }

            var pendingQuestions = _dataFactory.PendingQuestionRepository.GetByQuestionID(question.ID);
            if (pendingQuestions.Any(x => x.Status == true))
            {
                return 2;
            }

            if (pendingQuestions.Any(x => x.Status == null))
            {
                return 1;
            }
            return 0;
        }

        private static int GetStatusForAnswer(Answer answer)
        {
            if (answer.Accepted == null)
            {
                return 1;
            }
            return answer.Accepted.GetValueOrDefault() ? 3 : 2;
        }

        private static int GetStatusForPending(PendingQuestion pending)
        {
            if (pending.Status == null)
            {
                return 1;
            }
            return pending.Status.GetValueOrDefault() ? 3 : 2;
        }
    }
}
