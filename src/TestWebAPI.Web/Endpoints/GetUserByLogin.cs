using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TestWebAPI.Core.UserAggregate;
using TestWebAPI.Core.UserAggregate.Specifications;
using TestWebAPI.SharedKernel.Interfaces;
using TestWebAPI.Web.ApiModels;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class GetUserByLogin : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<GetUserByLoginResponse> {

        private readonly IRepository<User> _repository;

        public GetUserByLogin(IRepository<User> repository) {
            _repository = repository;
        }

        [Authorize]
        [HttpGet("/api/UserByLogin")]
        [SwaggerOperation(
            Summary = "Returns user by login",
            Description = "Returns user by login. For Admins only",
            OperationId = "User.UserByLogin",
            Tags = new[] { "UserEndpoints" })
        ]
        public override async Task<ActionResult<GetUserByLoginResponse>> HandleAsync([FromQuery] string login,
            CancellationToken cancellationToken) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var response = new GetUserByLoginResponse();

            string userLogin = User.FindFirst(ClaimTypes.Name)?.Value;
            var userSpec = new UserByLogin(userLogin);
            var user = await _repository.GetBySpecAsync(userSpec);

            if (user == null) {
                response.ErrorMessage = "Some internal error.";
                response.Result = false;
                return response;
            }

            if (!user.Admin) {
                response.ErrorMessage = "Not allowed.";
                response.Result = false;
                return response;
            }

            var userToGetSpec = new UserByLogin(login);
            var userToGet = await _repository.GetBySpecAsync(userToGetSpec);

            if (userToGet == null) {
                response.ErrorMessage = $"User with login: {login} wasn't found.";
                response.Result = false;
                return response;
            }

            response.Result = true;

            response.User = new UserDTO(
                    login: userToGet.Login,
                    name: userToGet.Name,
                    gender: userToGet.Gender,
                    birhday: userToGet.Birthday,
                    createdBy: userToGet.CreatedBy,
                    modifiedBy: userToGet.ModifiedBy,
                    modifiedOn: userToGet.ModifiedOn,
                    revokedBy: userToGet.RevokedBy,
                    revokedOn: userToGet.RevokedOn
                    );

            return Ok(response);
        }
    }
}
