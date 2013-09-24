using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Radvill.Services.Advisor;
using Radvill.Services.DataFactory;
using Radvill.WebAPI.Models;

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

        public HttpResponseMessage Get()
        {
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
