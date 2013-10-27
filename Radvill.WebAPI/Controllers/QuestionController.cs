using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Radvill.Models.AdviseModels;
using Radvill.Services.Advisor;
using Radvill.Services.DataFactory;
using Radvill.WebAPI.Models.Advise;

namespace Radvill.WebAPI.Controllers
{
    [Authorize]
    public class QuestionController : ApiController
    {
        private readonly IAdviseManager _adviseManager;
        private readonly IDataFactory _dataFactory;

        public QuestionController(IAdviseManager adviseManager, IDataFactory dataFactory)
        {
            _adviseManager = adviseManager;
            _dataFactory = dataFactory;
        }

        public HttpResponseMessage Get()
        {
            var user = _dataFactory.UserRepository.GetUserByEmail(User.Identity.Name);

            var questions = user.Questions.Select(x => new GetQuestionDTO
            {
                ID = x.ID,
                Category = x.Category.Name,
                Status = GetStatusForQuestion(x),
                Question = x.Text,
                TimeStamp = x.TimeStamp,
                Answers = x.Answers.Select(y => new AnswerDTO{Accepted = y.Accepted, ID = y.ID, Text = y.Text})
            }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, questions.OrderByDescending(x => x.TimeStamp));

        }

        public HttpResponseMessage Get(int id)
        {

            if (ModelState.IsValid)
            {
                var question = _dataFactory.QuestionRepository.GetByID(id);
                if (question.User.Email == User.Identity.Name)
                {
                    var questionDto = new GetQuestionDTO
                    {
                        ID = question.ID,
                        Category = question.Category.Name,
                        Status = GetStatusForQuestion(question),
                        Question = question.Text,
                        TimeStamp = question.TimeStamp,
                        Answers = question.Answers.Select(y => new AnswerDTO { Accepted = y.Accepted, ID = y.ID, Text = y.Text })
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, questionDto);
                }
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        public HttpResponseMessage Post([FromBody] SubmitQuestionDTO questionDto)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.Identity.Name;
                var userId = _dataFactory.UserRepository.GetUserByEmail(userEmail).ID;

                return _adviseManager.SubmitQuestion(userId, questionDto.CategoryID, questionDto.Question) 
                    ? Request.CreateResponse(HttpStatusCode.Created, true) 
                    : Request.CreateResponse(HttpStatusCode.OK, false);
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
    }
}
