namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class GetUsersByAgeRequest {
        public const string Route = "/api/Users/{Age:int}";

        public static string BuildRoute(int age) => Route.Replace("{Age:int}", age.ToString());

        public int Age { get; set; }

    }
}
