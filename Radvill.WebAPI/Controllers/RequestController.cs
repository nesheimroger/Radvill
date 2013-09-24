using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Radvill.Services.Advisor;
using Radvill.Services.DataFactory;
using Radvill.WebAPI.Models;

namespace Radvill.WebAPI.Controllers
{
    [Authorize]
    public class RequestController : ApiController
    {
        private readonly IAdviseManager _adviseManager;
        private readonly IDataFactory _dataFactory;

        public RequestController(IAdviseManager adviseManager, IDataFactory dataFactory)
        {
            _adviseManager = adviseManager;
            _dataFactory = dataFactory;
        }

        public HttpResponseMessage Post([FromBody] RequestDTO requestDto)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.Identity.Name;
                var userId = _dataFactory.UserRepository.GetUserByEmail(userEmail).ID;

                return _adviseManager.SubmitQuestion(userId, requestDto.CategoryID, requestDto.Question) 
                    ? Request.CreateResponse(HttpStatusCode.Created, true) 
                    : Request.CreateResponse(HttpStatusCode.OK, false);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);

        }
    }
}
