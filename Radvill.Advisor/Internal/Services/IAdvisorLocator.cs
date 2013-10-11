using Radvill.Models.UserModels;

namespace Radvill.Advisor.Internal.Services
{
    public interface IAdvisorLocator
    {
        User GetNextInLine(int questionId);
    }
}