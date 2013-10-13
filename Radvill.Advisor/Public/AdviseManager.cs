using System;
using System.Linq;
using System.Threading.Tasks;
using Radvill.Advisor.Internal.Services;
using Radvill.Models.AdviseModels;
using Radvill.Models.UserModels;
using Radvill.Services.Advisor;
using Radvill.Services.DataFactory;
using Radvill.Services.Sockets;

namespace Radvill.Advisor.Public
{
    public class AdviseManager : IAdviseManager
    {

        private readonly IDataFactory _dataFactory;
        private readonly IAdvisorLocator _advisorLocator;
        private readonly IEventManager _eventManager;

        public AdviseManager(IDataFactory dataFactory, IAdvisorLocator advisorLocator, IEventManager eventManager)
        {
            _dataFactory = dataFactory;
            _advisorLocator = advisorLocator;
            _eventManager = eventManager;
        }

        public bool SubmitQuestion(int userid, int categoryId, string question)
        {
            try
            {
                var user = _dataFactory.UserRepository.GetByID(userid);
                var category = _dataFactory.CategoryRepository.GetByID(categoryId);
                var timeStamp = DateTime.Now;
                
                var questionEntity = new Question
                {
                    Text = question,
                    Category = category,
                    User = user,
                    TimeStamp = timeStamp
                };

                _dataFactory.QuestionRepository.Insert(questionEntity);
                _dataFactory.Commit();

                var reciever = _advisorLocator.GetNextInLine(questionEntity.ID);

                if (reciever == null)
                {
                    _dataFactory.QuestionRepository.Delete(questionEntity);
                    _dataFactory.Commit();
                    return false;
                }

                var pendingEntity = new PendingQuestion
                {
                    Question = questionEntity,
                    Status = null,
                    User = reciever,
                    TimeStamp = timeStamp
                };

                _dataFactory.PendingQuestionRepository.Insert(pendingEntity);
                _dataFactory.Commit();
                _eventManager.QuestionAssigned(pendingEntity);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log.Fatal("Exception during Submit Question", e);
                throw;
            }

        }

        public void PassQuestion(PendingQuestion pendingQuestion)
        {
            try
            {
                //Set pass status
                pendingQuestion.Status = false;
                _dataFactory.PendingQuestionRepository.Update(pendingQuestion);

                var reciever = _advisorLocator.GetNextInLine(pendingQuestion.Question.ID);
                if (reciever == null)
                {
                    _dataFactory.Commit();
                    _eventManager.AllRecipientsPassed(pendingQuestion.Question);
                    return;
                }

                var newPending = new PendingQuestion
                {
                    Question = pendingQuestion.Question,
                    Status = null,
                    TimeStamp = DateTime.Now,
                    User = reciever
                };

                _dataFactory.PendingQuestionRepository.Insert(newPending);
                _dataFactory.Commit();
                _eventManager.QuestionAssigned(newPending);
            }
            catch (Exception e)
            {
                Logger.Log.Fatal("Exception during Pass Question", e);
                throw;
            }

            

        }

        public void PassQuestionForUser(string email)
        {
            var user = _dataFactory.UserRepository.GetUserByEmail(email);
            var pending = user.PendingQuestions.Where(x => x.Status != false && x.Answer == null).ToList();
            foreach (var pendingQuestion in pending)
            {
                PassQuestion(pendingQuestion);
            }
        }

        public bool StartAnswer(PendingQuestion pending)
        {
            try
            {
                if (DateTime.Now > GetDeadline(pending))
                {
                    return false;
                }
                pending.Status = true;
                _dataFactory.PendingQuestionRepository.Update(pending);
                _dataFactory.Commit();
                _eventManager.AnswerStarted(pending);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log.Fatal("Exception during start answer", e);
                throw;
            }
            
        }

        public DateTime GetDeadline(PendingQuestion pending)
        {
            return pending.Status == true 
                ? pending.TimeStamp.AddSeconds(Configuration.Timeout.Respond + Configuration.Timeout.Answer) 
                : pending.TimeStamp.AddSeconds(Configuration.Timeout.Respond);
        }

        public bool SubmitAnswer(PendingQuestion pending, string answer)
        {
            try
            {
                var now = DateTime.Now;
                if (now > GetDeadline(pending) || pending.Answer != null)
                {
                    return false;
                }

                var answerEntity = new Answer
                    {
                        Accepted = null,
                        Question = pending.Question,
                        Text = answer,
                        User = pending.User,
                        TimeStamp = now
                    };
                pending.Answer = answerEntity;
                _dataFactory.PendingQuestionRepository.Update(pending);
                _dataFactory.AnswerRepository.Insert(answerEntity);
                _dataFactory.Commit();
                _eventManager.AnswerSubmitted(answerEntity);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log.Fatal("Exception during submit answer", e);
                throw;
                
            }
            

        }
    }
}