// using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ECommerce.API.Controllers;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace ECommerce.Tests
{
    public class AuthControllerTests
    {
        [Fact]
        public void Register_SendsGoodRequestWhenProvidedProperNewRegisterRequest()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.CreateNewUser(It.IsAny<User>())).Returns(Task.FromResult<Boolean>(true));
            cMock.Setup(m => m.GetUserByEmailAndPassword("timtest@gmail.com", "testing")).Returns(Task.FromResult(new User { firstName = "Timmy", lastName = "Test", email = "timtest@gmail.com", password = "testing" }));
            AuthController controller = new AuthController(cMock.Object);
            RegisterRequest regReq = new RegisterRequest { firstName = "Timmy", lastName = "Test", email = "timtest@gmail.com", password = "testing" };

            var result = controller.Register(regReq);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void Register_SendsBadRequestWhenRegisterRequestHasImproperEmail()
        {
            var cMock = new Mock<IContext>();
            AuthController controller = new AuthController(cMock.Object);
            RegisterRequest regReq = new RegisterRequest { firstName = "Timmy", lastName = "Test", email = "timtest", password = "testing" };

            var result = controller.Register(regReq);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void Register_SendsBadRequestWhenRegisterRequestHasExistingEmail()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.CreateNewUser(It.IsAny<User>())).Returns(Task.FromResult<Boolean>(false));
            AuthController controller = new AuthController(cMock.Object);
            RegisterRequest regReq = new RegisterRequest { email = "test@gmail.com", password = "pa55", firstName = "Jon", lastName = "Dow" };

            var result = controller.Register(regReq);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void Login_SendsGoodRequestWhenProvidedProperUserCredentials()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.GetUserByEmailAndPassword("test@gmail.com", "pass")).Returns(Task.FromResult(new User { userId = 1, email = "test@gmail.com", password = "pass", firstName = "John", lastName = "Doe"}));
            AuthController controller = new AuthController(cMock.Object);
            LoginRequest logReq = new LoginRequest { email = "test@gmail.com", password = "pass" };

            var result = controller.Login(logReq);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void Login_SendsBadRequestWhenEmailIsCorrectButPasswordIsNot()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.GetUserByEmailAndPassword("test@gmail.com", "past")).Returns(Task.FromResult<User>(null));
            AuthController controller = new AuthController(cMock.Object);
            LoginRequest logReq = new LoginRequest { email = "test@gmail.com", password = "past" };

            var result = controller.Login(logReq);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void Login_SendsBadRequestWhenPasswordIsCorrectButEmailIsNot()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.GetUserByEmailAndPassword("tess@gmail.com", "pass")).Returns(Task.FromResult<User>(null));
            AuthController controller = new AuthController(cMock.Object);
            LoginRequest logReq = new LoginRequest { email = "tess@gmail.com", password = "pass" };

            var result = controller.Login(logReq);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void Login_SendsBadRequestWhenProperRequestButDbIsEmpty()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.GetUserByEmailAndPassword("test@gmail.com", "pass")).Returns(Task.FromResult<User>(null));
            AuthController controller = new AuthController(cMock.Object);
            LoginRequest logReq = new LoginRequest { email = "test@gmail.com", password = "pass" };

            var result = controller.Login(logReq);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void ResetPassword_SendsNewUserObjectWhenProvidedUserAndNewPassword()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.UpdateUserPassword("test@gmail.com", "newPass")).Returns(Task.FromResult(new User { userId = 1, email = "test@gmail.com", password = "newPass", firstName = "John", lastName = "Doe" }));
            cMock.Setup(m => m.CommitChangesAsync());
            cMock.Setup(m => m.DenoteUserModified(It.IsAny<User>()));
            AuthController controller = new AuthController(cMock.Object);
            UserDTO passReq = new UserDTO { email = "test@gmail.com", password = "newPass" };

            var result = controller.ResetPassword(passReq);

            Assert.IsType<User>(result.Result.Value);
        }

        [Fact]
        public void ResetPassword_SendsNullObjectWhenProvidedUserNotInDb()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.UpdateUserPassword("best@gmail.com", "pass")).Returns(Task.FromResult<User>(null));
            cMock.Setup(m => m.CommitChangesAsync());
            cMock.Setup(m => m.DenoteUserModified(It.IsAny<User>()));
            AuthController controller = new AuthController(cMock.Object);
            UserDTO passReq = new UserDTO { email = "best@gmail.com", password = "pass" };

            var result = controller.ResetPassword(passReq);

            Assert.Null(result.Result.Value);
        }

        [Fact]
        public void ResetPassword_SendsNullObjectWhenPasswordInRequestIsNotChanged()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.UpdateUserPassword("test@gmail.com", "pass")).Returns(Task.FromResult<User>(null));
            cMock.Setup(m => m.CommitChangesAsync());
            cMock.Setup(m => m.DenoteUserModified(It.IsAny<User>()));
            AuthController controller = new AuthController(cMock.Object);
            UserDTO passReq = new UserDTO { email = "test@gmail.com", password = "pass" };

            var result = controller.ResetPassword(passReq);

            Assert.Null(result.Result.Value);
        }

        [Fact]
        public void ResetPassword_SendsNullObjectWhenRequestIsGoodButDbIsEmpty()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.UpdateUserPassword("test@gmail.com", "newPass")).Returns(Task.FromResult<User>(null));
            cMock.Setup(m => m.CommitChangesAsync());
            cMock.Setup(m => m.DenoteUserModified(It.IsAny<User>()));
            AuthController controller = new AuthController(cMock.Object);
            UserDTO passReq = new UserDTO("test@gmail.com", "newPass");

            var result = controller.ResetPassword(passReq);

            Assert.Null(result.Result.Value);
        }

        [Fact]
        public void Logout_SendsGoodReponseAlways()
        {
            var cMock = new Mock<IContext>();
            AuthController controller = new AuthController(cMock.Object);

            var result = controller.Logout();

            Assert.IsType<OkResult>(result);
        }
    }
}
