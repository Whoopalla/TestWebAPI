using TestWebAPI.Web.ApiModels;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class GetUserByLoginResponse {

        public GetUserByLoginResponse() {
        }

        public bool Result { get; set; }
        public UserDTO User { get; set; }
        public string ErrorMessage { get; set; } = String.Empty;
    }
}
