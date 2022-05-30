using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class ChangeLoginRequest {
        public const string Route = "/api/ChangeLogin";

        [Required]
        public string Login { get; set; }
        [Required]
        public string NewLogin { get; set; }
    }
}