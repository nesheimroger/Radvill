using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using Radvill.Services.Security;
using Radvill.WebAPI.Models;
using Radvill.WebAPI.Models.Auth;

namespace Radvill.WebAPI.Controllers
{
    public class LoginController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Attempts to login using the specified credentials
        /// </summary>
        /// <param name="loginDto">Login credentials</param>
        /// <returns>True / False</returns>
        public HttpResponseMessage Post([FromBody] LoginDTO loginDto)
        {
            if (_authenticationService.VerifyCredentials(loginDto.Email, loginDto.Password))
            {
                FormsAuthentication.SetAuthCookie(loginDto.Email, true);
                return Request.CreateResponse(true);
            }
            return Request.CreateResponse(false);
        }

        /// <summary>
        /// Returns wheter or not a user is authenticated
        /// </summary>
        /// <returns>True / False</returns>
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(User.Identity.IsAuthenticated);
        }

        public HttpResponseMessage Delete()
        {
            //TODO: Implement logout.
            throw new NotImplementedException();
        }
    }
}
