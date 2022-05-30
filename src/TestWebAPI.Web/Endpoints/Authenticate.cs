using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TestWebAPI.Core.Interfaces;
using TestWebAPI.Core.UserAggregate;
using TestWebAPI.Core.UserAggregate.Specifications;
using TestWebAPI.SharedKernel.Interfaces;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class Authenticate : BaseAsyncEndpoint
        .WithRequest<AuthenticateRequest>
        .WithResponse<AuthenticateResponse> {

        private readonly ITokenService _tokenClaimsService;
        private readonly IRepository<User> _repository;

        public Authenticate(ITokenService tokenClaimsService,
            IRepository<User> repository) {
            _tokenClaimsService = tokenClaimsService;
            _repository = repository;
        }

        [HttpPost("/api/Authenticate")]
        [SwaggerOperation(
            Summary = "Authenticates user",
            Description = "Authenticates user. Returns token.",
            OperationId = "User.Authenticate",
            Tags = new[] { "UserEndpoints" })
        ]
        public override async Task<ActionResult<AuthenticateResponse>> HandleAsync([FromBody] AuthenticateRequest request,
            CancellationToken cancellationToken) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var response = new AuthenticateResponse();
            var spec = new UserByLogin(request.Login);
            var user = await _repository.GetBySpecAsync(spec);

            if (user == null) {
                response.Result = false;
                response.Message = $"User was not found.";
                return NotFound(response);
            }

            else if (user.Password != request.Password) {
                response.Result = false;
                response.Login = user.Login;
                response.Message = $"Wrong password.";
                return response;
            }

            else if (user.RevokedOn != null) {
                response.Message = $"User with Login {request.Login} was deleted.";
                response.Result = false;
                response.Login = user.Login;
                response.Message = $"Wrong password.";
                return response;
            }

            var token = await _tokenClaimsService.GetTokenAsync(request.Login);
            response.Token = token;

            response.Result = true;
            response.Login = user.Login;

            return Ok(response);
        }
    }
}
