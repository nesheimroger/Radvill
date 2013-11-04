using Radvill.Models.UserModels;

namespace Radvill.Models.ScoreModels
{
    public class Score
    {
        public AdvisorProfile Advisor { get; set; }
        public int Points { get; set; }
    }
}