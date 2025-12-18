using Xunit;
using Moq;
using TradingCompany.WPF.Services.Concrete;
using TradingCompany.DALEF.Interfaces;
using Assert = Xunit.Assert;

namespace wpftest.Services
{
    public class AuthenticationTests
    {
        private readonly Mock<IUserDAL> _mockUserDal;
        private readonly Mock<IUserRoleDAL> _mockUserRoleDal;
        private readonly Authentication _authService;

        public AuthenticationTests()
        {
            _mockUserDal = new Mock<IUserDAL>();
            _mockUserRoleDal = new Mock<IUserRoleDAL>();
            _authService = new Authentication(_mockUserDal.Object, _mockUserRoleDal.Object);
        }

        [Fact]
        public void ValidateUser_ShouldReturnTrue_WhenCredentialsAreCorrect()
        {
            string login = "testUser";
            string password = "password123";
            string expectedRole = "Admin";

            _mockUserDal.Setup(dal => dal.ValidateUser(login, password))
                        .Returns(true);

            _mockUserRoleDal.Setup(roleDal => roleDal.GetRoleNameByLogin(login))
                            .Returns(expectedRole);
            bool result = _authService.ValidateUser(login, password, out string actualRole);
            Assert.True(result, "Метод повинен повернути true, якщо дані правильні");
            Assert.Equal(expectedRole, actualRole);
        }

        [Fact]
        public void ValidateUser_ShouldReturnFalse_WhenCredentialsAreInvalid()
        {
            string login = "wrongUser";
            string password = "wrongPassword";

            _mockUserDal.Setup(dal => dal.ValidateUser(login, password))
                        .Returns(false);

            bool result = _authService.ValidateUser(login, password, out string actualRole);

            Assert.False(result, "Метод повинен повернути false, якщо пароль неправильний");
            Assert.Equal("User", actualRole);
        }

        [Fact]
        public void ValidateUser_ShouldNotCallGetRole_WhenCredentialsAreInvalid()
        {
            string login = "badUser";
            string password = "badPassword";

            _mockUserDal.Setup(dal => dal.ValidateUser(login, password))
                        .Returns(false);

            _authService.ValidateUser(login, password, out _);

            _mockUserRoleDal.Verify(x => x.GetRoleNameByLogin(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void ValidateUser_ShouldCallGetRole_Once_WhenCredentialsAreValid()
        {
            string login = "goodUser";
            string password = "goodPassword";

            _mockUserDal.Setup(dal => dal.ValidateUser(login, password)).Returns(true);
            _mockUserRoleDal.Setup(dal => dal.GetRoleNameByLogin(login)).Returns("Manager");

            _authService.ValidateUser(login, password, out _);

            _mockUserRoleDal.Verify(x => x.GetRoleNameByLogin(login), Times.Once);
        }
    }
}