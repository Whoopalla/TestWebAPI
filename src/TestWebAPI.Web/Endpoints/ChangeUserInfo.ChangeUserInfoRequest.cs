using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class ChangeUserInfoRequest {
        public const string Route = "/api/ChangeUserInfo";

        [Required]
        public string Login { get; set; }
        public string NewName { get; set; }
        public int? NewGender { get; set; }
        public DateTime? NewBirthday { get; set; }
    }
}