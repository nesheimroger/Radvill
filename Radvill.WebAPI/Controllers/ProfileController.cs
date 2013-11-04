using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Radvill.Models.UserModels;
using Radvill.Services.DataFactory;
using Radvill.Services.Scores;
using Radvill.WebAPI.Models.Profile;

namespace Radvill.WebAPI.Controllers
{
    [Authorize]
    public class ProfileController : ApiController
    {
        private readonly IDataFactory _dataFactory;
        private readonly IScoreKeeper _scoreKeeper;

        public ProfileController(IDataFactory dataFactory, IScoreKeeper scoreKeeper)
        {
            _dataFactory = dataFactory;
            _scoreKeeper = scoreKeeper;
        }

        public HttpResponseMessage Get()
        {
            var user =  _dataFactory.UserRepository.GetUserByEmail(User.Identity.Name);
            var points = _scoreKeeper.GetPointsByUser(User.Identity.Name);

            var dto = new ProfileDTO
                {
                    DisplayName = user.AdvisorProfile.DisplayName,
                    Description = user.AdvisorProfile.Description,
                    Points = points
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
