using System.Collections.Generic;
using Radvill.Models.UserModels;

namespace Radvill.Services.DataFactory.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByEmail(string email);
        List<User> GetAvailableUsers();
    }
}