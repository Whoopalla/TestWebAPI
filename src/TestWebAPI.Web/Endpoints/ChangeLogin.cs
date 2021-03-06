using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TestWebAPI.Core.UserAggregate;
using TestWebAPI.Core.UserAggregate.Specifications;
using TestWebAPI.SharedKernel.Interfaces;
using TestWebAPI.Web.ApiModels;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class ChangeLogin : BaseAsyncEndpoint
        .WithRequest<ChangeLoginRequest>
        .WithResponse<ChangeLoginResponse> {

        private readonly IRepository<User> _repository;
        private readonly ILogger<ChangePassword> _logger;

        public ChangeLogin(IRepository<User> repository,
            ILogger<ChangePassword> logger) {
            _repository = repository;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("/api/ChangeLogin")]
        [SwaggerOperation(
            Summary = "Changes user's login",
            Description = "Changes user's login.",
            OperationId = "User.ChangeLogin",
            Tags = new[] { "UserEndpoints" })
        ]
        public override async Task<ActionResult<ChangeLoginResponse>> HandleAsync([FromBody] ChangeLoginRequest request,
            CancellationToken cancellationToken) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var response = new ChangeLoginResponse();

            string userLogin = User.FindFirst(ClaimTypes.Name)?.Value;
            var userSpec = new UserByLogin(userLogin);
            var user = await _repository.GetBySpecAsync(userSpec);

            if (user == null) {
                response.ErrorMessage = "Some internal error.";
                response.Result = false;
                return response;
            }

            var userToChangeSpec = new UserByLogin(request.Login);
            var userToChange = await _repository.GetBySpecAsync(userToChangeSpec);

            if (userToChange == null) {
                response.ErrorMessage = $"User with login: {request.Login} wasn't found.";
                response.Result = false;
                return response;
            }
            // check if user wasn't deleted
            if (!user.Admin && user.Id == userToChange.Id && user.RevokedOn != null) {
                response.ErrorMessage = $"The user was deleted at {user.RevokedOn} by {user.RevokedBy}.";
                response.Result = false;
                return response;
            }
            // check if user are to change his info, not anyone else's
            if (!user.Admin && user.Id != userToChange.Id && user.RevokedOn == null) {
                response.ErrorMessage = $"Not authorize.";
                response.Result = false;
                return response;
            }

            userToChange.Login = request.NewLogin;
            userToChange.ModifiedBy = user.Login;
            userToChange.ModifiedOn = DateTime.Now;

            try {
                await _repository.UpdateAsync(userToChange);
            }

            catch (DbUpdateException ex) {
                response.ErrorMessage = "Update wasn't successfull.";
                _logger.LogError($"There was error while trying to update user. {ex.Message}");
                return Ok(response);
            }

            response.Result = true;

            response.UpdatedUser = new UserDTO(
                    login: userToChange.Login,
                    name: userToChange.Name,
                    gender: userToChange.Gender,
                    birhday: userToChange.Birthday,
                    createdBy: userToChange.CreatedBy,
                    modifiedBy: userToChange.ModifiedBy,
                    modifiedOn: userToChange.ModifiedOn,
                    revokedBy: userToChange.RevokedBy,
                    revokedOn: userToChange.RevokedOn
                    );

            return Ok(response);
        }
    }
}
