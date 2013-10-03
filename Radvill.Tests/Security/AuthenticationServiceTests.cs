using Moq;
using NUnit.Framework;
using Radvill.Models.UserModels;
using Radvill.Security.Public;
using Radvill.Services.DataFactory;
using Radvill.Services.DataFactory.Repositories;
using Radvill.Services.Security;
using Radvill.Services.Security.StatusCodes;

namespace Radvill.Tests.Security
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IDataFactory> _dataFactory;
        private IAuthenticationService _authenticationService;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _dataFactory = new Mock<IDataFactory>();
            _dataFactory.Setup(x => x.UserRepository).Returns(_userRepository.Object);
            _authenticationService = new AuthenticationService(_dataFactory.Object);
        }

        [Test]
        public void CreateUser_ShouldAddUserToDb()
        {
            //Arrange
             

            //Act
            var result = _authenticationService.CreateUser("Test", "test@test.com", "password");

            //Assert
            Assert.That(result, Is.EqualTo(CreateUserStatus.Success));
            _userRepository.Verify(x => x.Insert(It.IsAny<User>()), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Once);
        }

        [Test]
        public void CreateUser_ShouldPreventDuplicateEmails()
        {
            //Arrange
            _userRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(new User());

            //Act
            var result = _authenticationService.CreateUser("Test", "test@test.com", "password");

            //Assert
            Assert.That(result, Is.EqualTo(CreateUserStatus.EmailAllreadyExists));
            _userRepository.Verify(x => x.Insert(It.IsAny<User>()), Times.Never);
            _dataFactory.Verify(x => x.Commit(), Times.Never);

        }

        [Test]
        public void VerifyCredentials_ShouldReturnTrue_IfCredentialsMatches()
        {
            //Arrange
            const string email = "test@test.com";
            const string password = "password";
            var hashedPassword = AuthenticationService.GetHashedPassword(email, password);
            _userRepository.Setup(x => x.GetUserByEmail(email)).Returns(new User {Email = email, Password = hashedPassword});

            //Act
            var result = _authenticationService.VerifyCredentials(email, password);

            //Assert
            Assert.That(result, Is.True);

        }

        [Test]
        public void VerifyCredentials_ShouldReturnFalse_IfCredentialsDontMatch()
        {
            //Arrange
            const string email = "test@test.com";
            const string password = "password";
            const string wrongPassword = "wrongPassord";
            var hashedPassword = AuthenticationService.GetHashedPassword(email, password);
            _userRepository.Setup(x => x.GetUserByEmail(email)).Returns(new User { Email = email, Password = hashedPassword });

            //Act
            var result = _authenticationService.VerifyCredentials(email, wrongPassword);

            //Assert
            Assert.That(result, Is.False);

        }

        [Test]
        public void VerifyCredentials_ShouldReturnFalse_IfUserDontExist()
        {
            //Arrange
            _userRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns((User)null);

            //Act
            var result = _authenticationService.VerifyCredentials("test@test.com", "test");

            //Assert
            Assert.That(result, Is.False);

        }
    }
}