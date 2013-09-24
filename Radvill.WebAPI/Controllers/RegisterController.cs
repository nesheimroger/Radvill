using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using Radvill.Services.Security;
using Radvill.Services.Security.StatusCodes;
using Radvill.WebAPI.Models;

namespace Radvill.WebAPI.Controllers
{
    public class RegisterController : ApiController
    {

        private readonly IAuthenticationService _authenticationService;

        public RegisterController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public HttpResponseMessage Post([FromBody] RegisterDTO registerDto)
        {
            var status = _authenticationService.CreateUser(registerDto.DisplayName, registerDto.Email, registerDto.Password);


            if (status == CreateUserStatus.Success)
            {
                FormsAuthentication.SetAuthCookie(registerDto.Email, true);
                return Request.CreateResponse(HttpStatusCode.Created, true);
            }

            if (status == CreateUserStatus.EmailAllreadyExists)
            {
                return Request.CreateResponse(HttpStatusCode.OK, false);
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error occured");

        }

       
    }
}
