using Radvill.Services.Security.StatusCodes;

namespace Radvill.Services.Security
{
    public interface IAuthenticationService
    {
        CreateUserStatus CreateUser(string displayName, string email, string password);

        bool VerifyCredentials(string email, string password);
    }
}