﻿using System.Collections.Generic;
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
            var inQue = Context.PendingQuestions
                .Where(x => x.Status != false)
                .Select(x => x.User);

            return Get(x => !inQue.Contains(x)).ToList();
        }
    }
}