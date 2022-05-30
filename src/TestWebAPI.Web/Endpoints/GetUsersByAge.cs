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
    public class GetUsersByAge : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<GetUsersByAgeResponce> {

        private readonly IRepository<User> _repository;

        public GetUsersByAge(IRepository<User> userRepository) {
            _repository = userRepository;
        }
        [Authorize]
        [HttpGet("/api/Users")]
        [SwaggerOperation(
            Summary = "Returns list of users",
            Description = "Returns list of users with age greter then Age parameter",
            OperationId = "User.Users",
            Tags = new[] { "UserEndpoints" })
        ]
        public override async Task<ActionResult<GetUsersByAgeResponce>> HandleAsync([FromQuery] int age,
            CancellationToken cancellationToken) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var response = new GetUsersByAgeResponce();

            string userLogin = User.FindFirst(ClaimTypes.Name)?.Value;
            var userSpec = new UserByLogin(userLogin);
            var user = await _repository.GetBySpecAsync(userSpec);

            if (user == null) {
                response.ErrorMessage = "Some internal error.";
                return response;
            }

            if (!user.Admin) {
                response.ErrorMessage = "You are not authorize to access this endpoint.";
                return response;
            }

            var spec = new AllUsersByAge(age);
            var usersDTOs = (await _repository.ListAsync(spec))
                .Select(user => new UserDTO(
                    login: user.Login,
                    name: user.Name,
                    gender: user.Gender,
                    birhday: user.Birthday,
                    createdBy: user.CreatedBy,
                    modifiedBy: user.ModifiedBy,
                    modifiedOn: user.ModifiedOn,
                    revokedBy: user.RevokedBy,
                    revokedOn: user.RevokedOn
                ))
                .ToList();

            if (usersDTOs == null) { return response; };

            response.Users = usersDTOs;
            return Ok(response);
        }
    }
}
