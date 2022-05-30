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
    public class ChangeUserInfo : BaseAsyncEndpoint
        .WithRequest<ChangeUserInfoRequest>
        .WithResponse<ChangeUserInfoResponse> {

        private readonly IRepository<User> _repository;
        private readonly ILogger<ChangeUserInfo> _logger;

        public ChangeUserInfo(IRepository<User> repository,
            ILogger<ChangeUserInfo> logger) {
            _repository = repository;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("/api/ChangeUserInfo")]
        [SwaggerOperation(
            Summary = "Changes user's info",
            Description = "Changes user's info. " +
            "Optional Name, Birhtday and Gender fields.",
            OperationId = "User.ChangeInfo",
            Tags = new[] { "UserEndpoints" })
        ]
        public override async Task<ActionResult<ChangeUserInfoResponse>> HandleAsync([FromBody] ChangeUserInfoRequest request,
            CancellationToken cancellationToken) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var response = new ChangeUserInfoResponse();

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
                response.ErrorMessage = $"You are only allowed to change your info.";
                response.Result = false;
                return response;
            }

            userToChange.Name = request.NewName == String.Empty || request.NewName == null
                    ? userToChange.Name : request.NewName;
            userToChange.Gender = (int)(request.NewGender == null
                    ? userToChange.Gender : request.NewGender);
            userToChange.Birthday = request.NewBirthday == null
                    ? userToChange.Birthday : request.NewBirthday;
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
