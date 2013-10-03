using System;
using System.Security.Cryptography;
using System.Text;
using Radvill.Models.UserModels;
using Radvill.Services.DataFactory;
using Radvill.Services.Security;
using Radvill.Services.Security.StatusCodes;

namespace Radvill.Security.Public
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly IDataFactory _dataFactory;

        public AuthenticationService(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        public CreateUserStatus CreateUser(string displayName, string email, string password)
        {
            try
            {
                if (_dataFactory.UserRepository.GetUserByEmail(email) != null)
                {
                    return CreateUserStatus.EmailAllreadyExists;
                }

                var user = new User
                {
                    Email = email,
                    Password = GetHashedPassword(email, password),
                    Created = DateTime.Now,
                    AdvisorProfile = new AdvisorProfile
                    {
                        DisplayName = displayName,
                        Description = string.Empty
                    }
                };

                _dataFactory.UserRepository.Insert(user);
                _dataFactory.Commit();
                return CreateUserStatus.Success;
            }
            catch (Exception e)
            {
                Logger.Log.Fatal("Exception during creation of a user.", e);
                return CreateUserStatus.Error;
            }
        }

        public bool VerifyCredentials(string email, string password)
        {
            try
            {
                var user = _dataFactory.UserRepository.GetUserByEmail(email);
                if (user == null)
                {
                    return false;
                }
                return user.Password == GetHashedPassword(email, password);
            }
            catch (Exception e)
            {
                Logger.Log.Fatal("Exception during verification of credentials", e);
                return false;
            }
            
        }

        public static string GetHashedPassword(string email, string password)
        {
            var salt = CreateSalt(email);
            var hashedPassword = HashPassword(salt, password);
            return hashedPassword;
        }

        private static string CreateSalt(string userEmail)
        {
            var saltKeyBytes = Encoding.Default.GetBytes(Configuration.Authentication.SaltKey);

            var hasher = new Rfc2898DeriveBytes(userEmail,saltKeyBytes, 10000);
            return Convert.ToBase64String(hasher.GetBytes(25));
        }

        private static string HashPassword(string salt, string password)
        {
            var hasher = new Rfc2898DeriveBytes(password,Encoding.Default.GetBytes(salt), 10000);
            return Convert.ToBase64String(hasher.GetBytes(25));

        }

       
    }
}