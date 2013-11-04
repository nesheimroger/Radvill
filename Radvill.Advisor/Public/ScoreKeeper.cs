using System.Collections.Generic;
using System.Linq;
using Radvill.Models.ScoreModels;
using Radvill.Models.UserModels;
using Radvill.Services.DataFactory;
using Radvill.Services.Scores;

namespace Radvill.Advisor.Public
{
    public class ScoreKeeper : IScoreKeeper
    {
        private readonly IDataFactory _dataFactory;

        public ScoreKeeper(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        public List<Score> GetHighscores()
        {
            var users = _dataFactory.UserRepository.GetAll();
            var scores = users.Select(user => new Score
                {
                    Advisor = user.AdvisorProfile, Points = CalculatePoints(user)
                }).OrderByDescending(x => x.Points).ToList();

            return scores;
        }

        public int GetPointsByUser(string email)
        {
            var user = _dataFactory.UserRepository.GetUserByEmail(email);
            return CalculatePoints(user);
        }

        private int CalculatePoints(User user)
        {
            var acceptedpoints = user.Answers.Count(x => x.Accepted == true) * Configuration.Points.Accepted;
            var declinedpoints = user.Answers.Count(x => x.Accepted == false) * Configuration.Points.Declined;
            return acceptedpoints + declinedpoints;
        }
    }
}
