using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Radvill.Services.Scores;
using Radvill.WebAPI.Models.Profile;

namespace Radvill.WebAPI.Controllers
{
    [Authorize]
    public class HighscoresController : ApiController
    {
        private readonly IScoreKeeper _scoreKeeper;

        public HighscoresController(IScoreKeeper scoreKeeper)
        {
            _scoreKeeper = scoreKeeper;
        }

        public HttpResponseMessage Get()
        {
            var highscores = _scoreKeeper.GetHighscores().Select(x => new ProfileDTO
                {
                    DisplayName = x.Advisor.DisplayName,
                    Description = x.Advisor.Description,
                    Points = x.Points
                });
            return Request.CreateResponse(HttpStatusCode.OK, highscores);
        }
    }
}
