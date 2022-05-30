namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class GetActiveUsersRequest {
        public const string Route = "/api/GetActiveUsers";

        public static string BuildRoute(int age) => Route.Replace("{Age:int}", age.ToString());

        public int Age { get; set; }

    }
}
