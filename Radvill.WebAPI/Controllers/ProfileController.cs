using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Radvill.Models.UserModels;
using Radvill.Services.DataFactory;
using Radvill.WebAPI.Models.Profile;

namespace Radvill.WebAPI.Controllers
{
    [Authorize]
    public class ProfileController : ApiController
    {
        private readonly IDataFactory _dataFactory;

        public ProfileController(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        public HttpResponseMessage Get([FromBody]int? id)
        {
            var user = id.HasValue 
                ? _dataFactory.UserRepository.GetByID(id.Value) 
                : _dataFactory.UserRepository.GetUserByEmail(User.Identity.Name);

            var dto = new ProfileDTO
                {
                    DisplayName = user.AdvisorProfile.DisplayName,
                    Description = user.AdvisorProfile.Description
                };

            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }

        public HttpResponseMessage Put([FromBody] ProfileDTO profileDTO)
        {
            if (ModelState.IsValid)
            {
                var user = _dataFactory.UserRepository.GetUserByEmail(User.Identity.Name);
                user.AdvisorProfile.DisplayName = profileDTO.DisplayName;
                user.AdvisorProfile.Description = profileDTO.Description;
                _dataFactory.UserRepository.Update(user);
                _dataFactory.Commit();
                return Request.CreateResponse(HttpStatusCode.OK);

            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);

        }
    }
}
