using Ardalis.Specification;

namespace TestWebAPI.Core.UserAggregate.Specifications {
    public class AllActiveUsers : Specification<User> {
        public AllActiveUsers() {
            Query
                .Where(user => user.RevokedOn == null);
        }
    }
}
