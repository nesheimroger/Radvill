using System;
using System.Collections.Generic;
using System.Linq;
using Radvill.Advisor.Private.Helpers;
using Radvill.Advisor.Private.Services;
using Radvill.Models.UserModels;
using Radvill.Services.DataFactory;

namespace Radvill.Advisor.Private.Components
{
    public class AdvisorLocator : IAdvisorLocator
    {

        private readonly IDataFactory _dataFactory;

        public AdvisorLocator(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        public User GetNextInLine()
        {
            return GetNextInLine(0);
        }

        public User GetNextInLine(int questionId)
        {
            var que = GetUserQue();

            return questionId == 0 
                ? que.FirstOrDefault() 
                : que.FirstOrDefault(x => x.PendingQuestions.Any(y => y.Question.ID != questionId));
        }

        private IEnumerable<User> GetUserQue()
        {
            return _dataFactory.UserRepository.GetAvailableUsers()
                .OrderBy(x => x.Answers.Any())
                .ThenByDescending(x => x.Answers.Select(y => (DateTime?) y.TimeStamp).Max(),new TimeStampComparer())
                .ThenByDescending(x => x.PendingQuestions.Select(y => (DateTime?) y.TimeStamp).Max(),new TimeStampComparer())
                .ThenBy(x => x.Created);
        }
    }
}