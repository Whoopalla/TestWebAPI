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
    public class DeleteUser : BaseAsyncEndpoint
        .WithRequest<DeleteUserRequest>
        .WithResponse<DeleteUserResponse> {

        private readonly IRepository<User> _repository;
        private readonly ILogger<DeleteUser> _logger;


        public DeleteUser(IRepository<User> repository,
            ILogger<DeleteUser> logger) {
            _repository = repository;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("/api/DeleteUser")]
        [SwaggerOperation(
            Summary = "Deletes user",
            Description = "Deletes user or makes him «Revoked»",
            OperationId = "User.Delete",
            Tags = new[] { "UserEndpoints" })
        ]
        public override async Task<ActionResult<DeleteUserResponse>> HandleAsync([FromBody] DeleteUserRequest request,
            CancellationToken cancellationToken) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var response = new DeleteUserResponse();

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

            var userToDeleteSpec = new UserByLogin(request.Login);
            var userToDelete = await _repository.GetBySpecAsync(userToDeleteSpec);

            if (userToDelete == null) {
                response.ErrorMessage = $"User with login: {request.Login} wasn't found.";
                response.Result = false;
                return response;
            }

            if (!request.SoftDeletion) {
                try {
                    await _repository.DeleteAsync(userToDelete);
                }

                catch (DbUpdateException ex) {
                    response.ErrorMessage = "Deletion wasn't successfull.";
                    _logger.LogError($"There was error while trying to delete user. {ex.Message}");
                    return Ok(response);
                }

                response.Result = true;

                return Ok(response);
            }

            if (userToDelete.RevokedOn != null) {
                response.ErrorMessage = $"User with login: {userToDelete.Login} was already revoked.";
                return Ok(response);
            }

            userToDelete.RevokedBy = user.Login;
            userToDelete.RevokedOn = DateTime.Now;

            try {
                await _repository.UpdateAsync(userToDelete);
            }

            catch (DbUpdateException ex) {
                response.ErrorMessage = "Deletion wasn't successfull.";
                _logger.LogError($"There was error while trying to update user. {ex.Message}");
                return Ok(response);
            }

            response.UpdatedUser = new UserDTO(userToDelete);
            response.Result = true;

            return Ok(response);
        }
    }
}
