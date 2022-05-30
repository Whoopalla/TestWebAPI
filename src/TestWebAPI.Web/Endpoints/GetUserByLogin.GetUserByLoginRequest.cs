using System.ComponentModel.DataAnnotations;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class GetUserByLoginRequest {
        public const string Route = "/api/UserByLogin/{Login:int}";
        public static string BuildRoute(int age) => Route.Replace("{Login:int}", age.ToString());

        [Required]
        public string Login { get; set; }
    }
}