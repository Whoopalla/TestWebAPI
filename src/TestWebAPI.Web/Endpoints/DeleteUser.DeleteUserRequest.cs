using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class DeleteUserRequest {
        public const string Route = "/api/DeleteUser";

        [Required]
        public string Login { get; set; }
        [Required]
        public bool SoftDeletion { get; set; }
    }
}