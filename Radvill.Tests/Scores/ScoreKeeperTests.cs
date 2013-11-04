using System.Collections.ObjectModel;
using Moq;
using NUnit.Framework;
using Radvill.Advisor.Public;
using Radvill.Models.AdviseModels;
using Radvill.Models.UserModels;
using Radvill.Services.DataFactory;
using Radvill.Services.Scores;

namespace Radvill.Tests.Scores
{
    [TestFixture]
    public class ScoreKeeperTests
    {
        private Mock<IDataFactory> _dataFactory;
        private IScoreKeeper _scoreKeeper;

        [SetUp]
        public void Setup()
        {
            _dataFactory = new Mock<IDataFactory>();
            _scoreKeeper = new ScoreKeeper(_dataFactory.Object);
        }

        //Point values are set in app.config

        [Test]
        public void GetPointsForUser_ShouldReturnCorrectPoints()
        {
            //Arrange
            var user = new User
                {
                    Answers = new Collection<Answer>
                        {
                            new Answer {Accepted = null},
                            new Answer {Accepted = false},
                            new Answer {Accepted = null},
                            new Answer {Accepted = false},
                            new Answer {Accepted = true},
                            new Answer {Accepted = true},
                            new Answer {Accepted = true}
                        }
                };

            _dataFactory.Setup(x => x.UserRepository.GetUserByEmail(It.IsAny<string>())).Returns(user);

            const int expectedPoints = 9;

            //Act
            var result = _scoreKeeper.GetPointsByUser("email");

            Assert.That(result, Is.EqualTo(expectedPoints));

        }

        [Test]
        public void GetHighScores_ShouldReturnCorrectScoreForAllUsers_OrderedByPoints()
        {
            //Arrange
            var user1 = new User
            {
                AdvisorProfile = new AdvisorProfile{ID = 1, Description = "description1", DisplayName = "name1"},
                Answers = new Collection<Answer>
                        {
                            new Answer {Accepted = true},
                            new Answer {Accepted = null},
                        }
            };
            var user2 = new User
            {
                AdvisorProfile = new AdvisorProfile { ID = 2, Description = "description2", DisplayName = "name2" },
                Answers = new Collection<Answer>
                        {
                            new Answer {Accepted = true},
                            new Answer {Accepted = true}
                        }
            };

            _dataFactory.Setup(x => x.UserRepository.GetAll()).Returns(new Collection<User>{user1, user2});

            const int expectedPointsUser1 = 5;
            const int expectedPointsUser2 = 10;

            //Act
            var result = _scoreKeeper.GetHighscores();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Advisor, Is.EqualTo(user2.AdvisorProfile));
            Assert.That(result[0].Points, Is.EqualTo(expectedPointsUser2));
            Assert.That(result[1].Advisor, Is.EqualTo(user1.AdvisorProfile));
            Assert.That(result[1].Points, Is.EqualTo(expectedPointsUser1));

        }
    }
}