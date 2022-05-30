using Ardalis.Specification;

namespace TestWebAPI.Core.UserAggregate.Specifications {
    public class AllUsersByAge : Specification<User> {
        public AllUsersByAge(int age) {
            Query
                .Where(user => user.Birthday.Value.Date < DateTime.Now.Date.AddYears(-age));
        }
    }
}
