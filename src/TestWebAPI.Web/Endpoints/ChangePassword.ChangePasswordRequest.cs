using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class ChangePasswordRequest {
        public const string Route = "/api/ChangePassword";

        [Required]
        public string Login { get; set; } = String.Empty;
        [Required]
        public string NewPassword { get; set; } = String.Empty;
    }
}