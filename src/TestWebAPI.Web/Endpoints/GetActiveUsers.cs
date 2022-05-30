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
    public class GetActiveUsers : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<GetActiveUsersResponce> {

        private readonly IRepository<User> _repository;

        public GetActiveUsers(IRepository<User> userRepository) {
            _repository = userRepository;
        }
        [Authorize]
        [HttpGet("/api/GetActiveUsers")]
        [SwaggerOperation(
            Summary = "Gets list of active users",
            Description = "Gets list of active users. For Admins only",
            OperationId = "User.GetActiveUsers",
            Tags = new[] { "UserEndpoints" })
        ]
        public override async Task<ActionResult<GetActiveUsersResponce>> HandleAsync(CancellationToken cancellationToken) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var response = new GetActiveUsersResponce();

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

            var spec = new AllActiveUsers();
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

            if (usersDTOs == null) { return Ok(response); };

            response.Users = usersDTOs;
            return Ok(response);
        }
    }
}
