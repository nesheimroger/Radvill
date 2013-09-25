using System.Collections.Generic;
using System.Linq;
using Radvill.DataFactory.Internal.Services;
using Radvill.Models.AdviseModels;
using Radvill.Services.DataFactory.Repositories;

namespace Radvill.DataFactory.Public.Repositories
{
    public class PendingQuestionRepository : GenericRepository<PendingQuestion>, IPendingQuestionRepository
    {
        public PendingQuestionRepository(IRadvillContext context) : base(context)
        {
        }

        public List<PendingQuestion> GetByQuestionID(int id)
        {
            return Get(x => x.Question.ID == id).ToList();
        }

        public PendingQuestion GetByUserIDAndQuestionId(int userId, int questionId)
        {
            return Get(x => x.Question.ID == questionId && x.User.ID == userId).FirstOrDefault();
        }
    }
}