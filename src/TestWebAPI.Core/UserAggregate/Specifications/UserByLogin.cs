using Ardalis.Specification;

namespace TestWebAPI.Core.UserAggregate.Specifications {
    public class UserByLogin : Specification<User>, ISingleResultSpecification {
        public UserByLogin(string login) {
            Query
                .Where(user => user.Login == login);
        }
    }
}
