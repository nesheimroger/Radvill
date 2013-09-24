using System;
using System.Linq;
using Radvill.Models.AdviseModels;
using Radvill.Models.UserModels;
using Radvill.Services.Advisor;
using Radvill.Services.DataFactory;

namespace Radvill.Advisor.Public
{
    public class AdviseManager : IAdviseManager
    {

        private readonly IDataFactory _dataFactory;

        public AdviseManager(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
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

                var reciever = _dataFactory.UserRepository.GetAvailableUsers().FirstOrDefault(x => x.ID != userid);
                if (reciever == null)
                {
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

                return true;
            }
            catch (Exception e)
            {
                Logger.Log.Fatal("Exception during Submit Question", e);
                throw;
            }
            


        }

    }
}