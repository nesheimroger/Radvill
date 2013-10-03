using Radvill.Models.UserModels;

namespace Radvill.Advisor.Private.Services
{
    public interface IAdvisorLocator
    {
        User GetNextInLine(int questionId);
    }
}