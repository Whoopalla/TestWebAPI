namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class AuthenticateResponse {

        public AuthenticateResponse() {
        }

        public bool Result { get; set; } = false;
        public string Token { get; set; } = String.Empty;
        public string Login { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;
    }
}
