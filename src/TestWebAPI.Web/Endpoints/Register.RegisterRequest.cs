using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class RegisterRequest {
        public const string Route = "/api/Register";

        [Required]
        [RegularExpression(@"^[A-Za-z0-9]+$",
         ErrorMessage = "Only latin characters and numbers are allowed")]
        public string Login { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^[A-Za-z0-9]+$",
         ErrorMessage = "Only latin characters and numbers are allowed")]
        public string Password { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^[A-Za-zА-Яа-я]+$",
         ErrorMessage = "Only latin and Cyrillic  characters")]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Admin { get; set; }
    }
}
