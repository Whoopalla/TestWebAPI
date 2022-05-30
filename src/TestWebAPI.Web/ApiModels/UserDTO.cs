using TestWebAPI.Core.UserAggregate;

namespace TestWebAPI.Web.ApiModels {
    public class UserDTO {
        public string Login { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }
        public string RevokedBy { get; set; } = string.Empty;
        public DateTime? RevokedOn { get; set; }
        // Some of the fields are there only for demonstration purpose
        public UserDTO(string login,
            string name, int gender,
            DateTime? birhday, string createdBy,
            string modifiedBy, DateTime? modifiedOn,
            string revokedBy, DateTime? revokedOn) {

            Login = login;
            Name = name;
            Gender = gender;
            Birthday = birhday;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            ModifiedOn = modifiedOn;
            RevokedBy = revokedBy;
            RevokedOn = revokedOn;
        }

        public UserDTO(User user) {
            Login = user.Login;
            Name = user.Name;
            Gender = user.Gender;
            Birthday = user.Birthday;
            CreatedBy = user.CreatedBy;
            ModifiedBy = user.ModifiedBy;
            ModifiedOn = user.ModifiedOn;
            RevokedBy = user.RevokedBy;
            RevokedOn = user.RevokedOn;
        }
    }
}
