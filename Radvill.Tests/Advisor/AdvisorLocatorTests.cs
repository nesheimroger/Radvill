using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Radvill.Advisor.Internal.Components;
using Radvill.Models.AdviseModels;
using Radvill.Models.UserModels;
using Radvill.Services.DataFactory;
using Radvill.Services.DataFactory.Repositories;

namespace Radvill.Tests.Advisor
{
    public class AdvisorLocatorTests
    {
        private Mock<IDataFactory> _dataFactoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private AdvisorLocator _advisorLocator;

        
            
        [SetUp]
        public void SetUp()
        {
            _dataFactoryMock = new Mock<IDataFactory>();
            _userRepositoryMock = new Mock<IUserRepository>();
            
        }

        [TearDown]
        public void TearDown()
        {
            _dataFactoryMock = null;
            _userRepositoryMock = null;
        }

        [Test]
        public void GetNextInLine_ShouldReturnUser_WhoWaitedLongest_Case1()
        {
            //Arrange
            #region Users

            var users = new List<User>
            {
                new User
                    {
                        ID = 1,
                        Created = new DateTime(2013, 1, 1),
                        Answers = new Collection<Answer>
                            {
                                new Answer
                                    {
                                        TimeStamp = new DateTime(2012, 11, 11)
                                    }
                            },
                        PendingQuestions = new Collection<PendingQuestion>(),
                        Connected = true,
                        Questions = new Collection<Question>()
                    },
                new User
                    {
                        ID = 2,
                        Created = new DateTime(2013, 2, 2),
                        Answers = new Collection<Answer>(),
                        PendingQuestions = new Collection<PendingQuestion>(),
                        Connected = true,
                        Questions = new Collection<Question>()
                    },
                new User
                    {
                        ID = 3,
                        Created = new DateTime(2013,3,3),
                        Answers = new Collection<Answer>(),
                        PendingQuestions = new Collection<PendingQuestion>
                            {
                                new PendingQuestion
                                    {
                                        TimeStamp = new DateTime(2013, 3, 3),
                                        Question = new Question
                                            {
                                                ID = 0
                                            }
                                    }
                            },
                        Connected = true,
                        Questions = new Collection<Question>()
                    },
                new User
                    {
                        ID = 4,
                        Created = new DateTime(2013, 3, 2),
                        Answers = new Collection<Answer>(),
                        PendingQuestions = new Collection<PendingQuestion>(),
                        Connected = true,
                        Questions = new Collection<Question>()
                    }
            };

            #endregion
            _userRepositoryMock.Setup(x => x.GetAvailableUsers()).Returns(users);
            _dataFactoryMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _advisorLocator = new AdvisorLocator(_dataFactoryMock.Object);
            const int expectedUserId = 2;

            //Act
            var result = _advisorLocator.GetNextInLine(1);

            //Assert
            Assert.That(result.ID, Is.EqualTo(expectedUserId));
        }

        [Test]
        public void GetNextInLine_ShouldReturnUser_WhoWaitedLongest_Case2()
        {
            //Arrange
            #region Users

            var users = new List<User>
            {
                new User
                    {
                        ID = 1,
                        Created = new DateTime(2013, 1, 1),
                        Answers = new Collection<Answer>
                            {
                                new Answer
                                    {
                                        TimeStamp = new DateTime(2012, 11, 11)
                                    }
                            },
                        PendingQuestions = new Collection<PendingQuestion>(),
                        Connected = true,
                        Questions = new Collection<Question>()
                    },
                new User
                    {
                        ID = 3,
                        Created = new DateTime(2012,03,03),
                        Answers = new Collection<Answer>(),
                        PendingQuestions = new Collection<PendingQuestion>
                            {
                                new PendingQuestion
                                    {
                                        TimeStamp = new DateTime(2013, 3, 3),
                                        Question = new Question
                                            {
                                                ID = 0
                                            }
                                    }
                            },
                        Connected = true,
                        Questions = new Collection<Question>()
                    },
                new User
                    {
                        ID = 4,
                        Created = new DateTime(2013, 3, 2),
                        Answers = new Collection<Answer>(),
                        PendingQuestions = new Collection<PendingQuestion>(),
                        Connected = true,
                        Questions = new Collection<Question>()
                    },
            };
            #endregion
            _userRepositoryMock.Setup(x => x.GetAvailableUsers()).Returns(users);
            _dataFactoryMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _advisorLocator = new AdvisorLocator(_dataFactoryMock.Object);
            const int expectedUserId = 4;

            //Act
            var result = _advisorLocator.GetNextInLine(1);

            //Assert
            Assert.That(result.ID, Is.EqualTo(expectedUserId));
        }

        [Test]
        public void GetNextInLine_ShouldReturnUser_WhoWaitedLongest_Case3()
        {
            //Arrange
            #region Users

            var users = new List<User>
            {
                new User
                    {
                        ID = 1,
                        Created = new DateTime(2013, 1, 1),
                        Answers = new Collection<Answer>
                            {
                                new Answer
                                    {
                                        TimeStamp = new DateTime(2012, 11, 11)
                                    }
                            },
                        PendingQuestions = new Collection<PendingQuestion>(),
                        Connected = true,
                        Questions = new Collection<Question>()
                    },
                new User
                    {
                        ID = 3,
                        Created = new DateTime(2013,03,03),
                        Answers = new Collection<Answer>(),
                        PendingQuestions = new Collection<PendingQuestion>()
                            {
                                new PendingQuestion
                                    {
                                        TimeStamp = new DateTime(2013, 3, 3),
                                        Question = new Question
                                            {
                                                ID = 0
                                            }
                                    }
                            },
                        Connected = true,
                        Questions = new Collection<Question>()
                    }
            };
            #endregion
            _userRepositoryMock.Setup(x => x.GetAvailableUsers()).Returns(users);
            _dataFactoryMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _advisorLocator = new AdvisorLocator(_dataFactoryMock.Object);
            const int expectedUserId = 3;

            //Act
            var result = _advisorLocator.GetNextInLine(1);

            //Assert
            Assert.That(result.ID, Is.EqualTo(expectedUserId));
        }

        [Test]
        public void GetNextInLine_ShouldReturnUser_WhoWaitedLongest_AndHaveNotRecivedAnyQuestionsBefore()
        {
            //Arrange
            const int otherQuestionId = 1;
            const int questionId = 2;

            #region Users

            var users = new List<User>
            {
                new User
                    {
                        ID = 1,
                        Created = new DateTime(2013, 1, 1),
                        Answers = new Collection<Answer>
                            {
                                new Answer
                                    {
                                        TimeStamp = new DateTime(2012, 11, 11)
                                    }
                            },
                        PendingQuestions = new Collection<PendingQuestion>()
                            {
                                new PendingQuestion
                                    {
                                        TimeStamp = new DateTime(2013, 3, 3),
                                        Question = new Question{ID = otherQuestionId}
                                    }
                            },
                        Connected = true,
                        Questions = new Collection<Question>()
                    },
                new User
                    {
                        ID = 2,
                        Created = new DateTime(2013, 2, 2),
                        Answers = new Collection<Answer>(),
                        PendingQuestions = new Collection<PendingQuestion>()
                            {
                                new PendingQuestion
                                    {
                                        TimeStamp = new DateTime(2013, 2, 2),
                                        Question = new Question{ID = questionId}
                                    }
                            },
                        Connected = true,
                        Questions = new Collection<Question>()
                    },
                new User
                    {
                        ID = 3,
                        Created = new DateTime(2013,03,03),
                        Answers = new Collection<Answer>(),
                        PendingQuestions = new Collection<PendingQuestion>()
                            {
                                new PendingQuestion
                                    {
                                        TimeStamp = new DateTime(2013, 3, 3),
                                        Question = new Question{ID = otherQuestionId}
                                    }
                            },
                        Connected = true,
                        Questions = new Collection<Question>()
                    }

            };

            #endregion
            _userRepositoryMock.Setup(x => x.GetAvailableUsers()).Returns(users);
            _dataFactoryMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _advisorLocator = new AdvisorLocator(_dataFactoryMock.Object);
            const int expectedUserId = 3;
            

            //Act
            var result = _advisorLocator.GetNextInLine(questionId);

            //Assert
            Assert.That(result.ID, Is.EqualTo(expectedUserId));
        
        }

        [Test]
        public void GetNextInLine_ShouldNotReturnUser_WhoHavePassedTheQuestionBefore()
        {
            //Arrange
            var question = new Question{ID = 1};
            var question2 = new Question{ID = 2};
            var user = new User { Answers = new Collection<Answer>(), Questions = new Collection<Question>(),Connected = true};
            var pending = new PendingQuestion {Question = question, Status = false, User = user};
            var pending2 = new PendingQuestion {Question = question2, Status = false, User = user};
            user.PendingQuestions = new Collection<PendingQuestion>{pending, pending2};

            var users = new List<User> {user};
            _userRepositoryMock.Setup(x => x.GetAvailableUsers()).Returns(users);
            _dataFactoryMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _advisorLocator = new AdvisorLocator(_dataFactoryMock.Object);

            //Act
            var result = _advisorLocator.GetNextInLine(question2.ID);

            //Assert
            Assert.That(result, Is.Null);

        }

        [Test]
        public void GetNextInLine_ShouldNotReturnUser_WhoHaveAnsweredheQuestionBefore()
        {
            //Arrange
            var question = new Question { ID = 1 };
            var answer = new Answer {ID = 1, Accepted = false, Question = question};
            question.Answers = new Collection<Answer>{answer};
            var user = new User { Answers = new Collection<Answer>{answer}, Connected = true };
            var pending = new PendingQuestion { Question = question, Status = true, User = user, Answer = answer};
            user.PendingQuestions = new Collection<PendingQuestion> { pending };

            var users = new List<User> { user };
            _userRepositoryMock.Setup(x => x.GetAvailableUsers()).Returns(users);
            _dataFactoryMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _advisorLocator = new AdvisorLocator(_dataFactoryMock.Object);

            //Act
            var result = _advisorLocator.GetNextInLine(question.ID);

            //Assert
            Assert.That(result, Is.Null);

        }

        [Test]
        public void GetNextInLine_ShouldNotReturnUser_WhoSubmittedQuestion()
        {
            //Arrange
            var question = new Question { ID = 1 };
            var user = new User { Questions = new Collection<Question>{question}, Answers = new Collection<Answer>(), Connected = true, PendingQuestions = new Collection<PendingQuestion>()};

            var users = new List<User> { user };
            _userRepositoryMock.Setup(x => x.GetAvailableUsers()).Returns(users);
            _dataFactoryMock.Setup(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _advisorLocator = new AdvisorLocator(_dataFactoryMock.Object);

            //Act
            var result = _advisorLocator.GetNextInLine(question.ID);

            //Assert
            Assert.That(result, Is.Null);

        }



    }
}