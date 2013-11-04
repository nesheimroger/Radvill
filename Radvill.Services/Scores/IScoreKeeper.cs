using System.Collections.Generic;
using Radvill.Models.ScoreModels;

namespace Radvill.Services.Scores
{
    public interface IScoreKeeper
    {
        List<Score> GetHighscores();
        int GetPointsByUser(string email);
    }
}