using TestWebAPI.Web.ApiModels;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class ChangePasswordResponse {

        public ChangePasswordResponse() {
        }

        public bool Result { get; set; }
        public UserDTO UpdatedUser { get; set; }
        public string ErrorMessage { get; set; } = String.Empty;
    }
}
