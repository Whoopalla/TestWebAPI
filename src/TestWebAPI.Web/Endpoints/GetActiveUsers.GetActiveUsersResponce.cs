using TestWebAPI.Web.ApiModels;

namespace TestWebAPI.Web.Endpoints.UserEndpoints {
    public class GetActiveUsersResponce {
        public List<UserDTO> Users { get; set; }
        public string ErrorMessage { get; set; } = String.Empty;
    }
}
