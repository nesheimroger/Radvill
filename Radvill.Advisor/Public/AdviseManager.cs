﻿using System;
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

        public bool PassQuestion(int userid, int questionId)
        {
            try
            {
                //Set pass status
                var previousPending = _dataFactory.PendingQuestionRepository.GetByUserIDAndQuestionId(userid, questionId);
                previousPending.Status = false;
                _dataFactory.PendingQuestionRepository.Update(previousPending);

                var reciever = _advisorLocator.GetNextInLine(questionId);
                if (reciever == null)
                {
                    _dataFactory.Commit();
                    return false;
                }

                var question = _dataFactory.QuestionRepository.GetByID(questionId);
                var newPending = new PendingQuestion
                {
                    Question = question,
                    Status = null,
                    TimeStamp = DateTime.Now,
                    User = reciever
                };

                _dataFactory.PendingQuestionRepository.Insert(newPending);
                _dataFactory.Commit();
                _eventManager.QuestionAssigned(newPending);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log.Fatal("Exception during Pass Question", e);
                throw;
            }

            

        }
    }
}