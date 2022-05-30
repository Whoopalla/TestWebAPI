using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class AuthenticateRequest {
        public const string Route = "/api/Authenticate";

        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}