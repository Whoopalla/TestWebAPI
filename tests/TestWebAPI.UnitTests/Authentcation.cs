using Microsoft.AspNetCore.Mvc;
using Moq;
using TestWebAPI.Core.UserAggregate;
using TestWebAPI.Core.UserAggregate.Specifications;
using TestWebAPI.Infrastructure;
using TestWebAPI.SharedKernel.Interfaces;
using Xunit;

namespace TestWebAPI.UnitTests {
    public class Authentcation {
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly TokenService _tokenService;

        public Authentcation() {
            var user = new User() {
                Id = new Guid("12345678-1234-1234-1234-123456789123"),
                Name = "Jhon",
                Login = "Jhon123",
                Password = "123",
                Admin = false,
                Birthday = new DateTime(2000, 5, 5),
                Gender = 1
            };

            _mockUserRepository = new Mock<IRepository<User>>();
            _mockUserRepository.Setup(x => x.GetBySpecAsync(It.IsAny<UserByLogin>(), default))
                .ReturnsAsync(user);
            _tokenService = new TokenService();
        }

        [Fact]
        public async Task CorrectValues() {
            var request = new TestWebAPI.Web.Endpoints.UserEndpoints.AuthenticateRequest();
            request.Login = "Jhon123";
            request.Password = "123";

            var handler = new TestWebAPI.Web.Endpoints.UserEndpoints.Authenticate(_tokenService, _mockUserRepository.Object);

            var result = await handler.HandleAsync(request, CancellationToken.None);

            var expectedToken = await _tokenService.GetTokenAsync("Jhon123");

            Assert.IsType<OkObjectResult>(result.Result as OkObjectResult);
        }
    }
}
