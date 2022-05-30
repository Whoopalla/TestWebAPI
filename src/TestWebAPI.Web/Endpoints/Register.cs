using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TestWebAPI.Core.UserAggregate;
using TestWebAPI.SharedKernel.Interfaces;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class Register : BaseAsyncEndpoint
        .WithRequest<RegisterRequest>
        .WithResponse<RegisterResponce> {

        private readonly IRepository<User> _repository;

        public Register(IRepository<User> userRepository) {
            _repository = userRepository;
        }

        [HttpPost("/api/Register")]
        [SwaggerOperation(
            Summary = "Creates new user",
            Description = "Creates new user",
            OperationId = "User.Register",
            Tags = new[] { "UserEndpoints" })
        ]
        public override async Task<ActionResult<RegisterResponce>> HandleAsync([FromBody] RegisterRequest request,
            CancellationToken cancellationToken) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = new User() {
                Login = request.Login,
                Name = request.Name,
                Password = request.Password,
                Gender = request.Gender,
                Birthday = request.Birthday,
                CreatedOn = DateTime.Now
            };

            var response = new RegisterResponce();

            try {
                var result = await _repository.AddAsync(user);
                // e.g. all right
                if (result.Name == user.Name) {
                    response.Result = true;
                    return Ok(response);
                }
            }

            catch (DbUpdateException ex) {
                response.ErrorMessage = "There is already user with that login.";
                return Ok(response);
            }

            response.ErrorMessage = "Something went wrong while register new user.";
            return BadRequest(response);
        }
    }
}
