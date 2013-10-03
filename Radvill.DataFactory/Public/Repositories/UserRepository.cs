using System.Collections.Generic;
using System.Linq;
using Radvill.DataFactory.Internal.Services;
using Radvill.Models.UserModels;
using Radvill.Services.DataFactory.Repositories;

namespace Radvill.DataFactory.Public.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IRadvillContext context) : base(context)
        {
        }

        public User GetUserByEmail(string email)
        {
            return Get(x => x.Email == email).FirstOrDefault();
        }

        public List<User> GetAvailableUsers()
        {
            //Seems it have to be this way, returns no entries if combined with where
            var pending = Context.PendingQuestions.ToList(); 

            var inQue = pending.Where(x => x.Status != false).Select(x => x.User.ID).Distinct();                  
            return Get(x => !inQue.Contains(x.ID)).Where(x => x.Connected).ToList();
        }
    }
}