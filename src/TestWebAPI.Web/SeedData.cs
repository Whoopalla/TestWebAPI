using Microsoft.EntityFrameworkCore;
using TestWebAPI.Core.UserAggregate;
using TestWebAPI.Infrastructure.Data;

namespace TestWebAPI.Web {
    public static class SeedData {

        public static readonly List<User> Users = new List<User>();

        public static readonly User User1 = new User {
            Name = "Admin",
            Login = "Admin123",
            Password = "123",
            Admin = true,
            Birthday = new DateTime(2001, 3, 5),
            CreatedBy = "God",
            CreatedOn = DateTime.Now,
            Gender = 1

        };
        public static readonly User User2 = new User {
            Name = "Mike",
            Login = "Mike123",
            Password = "123",
            Admin = false,
            Birthday = new DateTime(2015, 3, 5),
            CreatedBy = "God",
            CreatedOn = DateTime.Now,
            Gender = 1

        };
        public static readonly User User3 = new User {
            Name = "Lucy",
            Login = "Lucy123",
            Password = "123",
            Admin = false,
            Birthday = new DateTime(1922, 3, 5),
            CreatedBy = "God",
            CreatedOn = DateTime.Now,
            Gender = 1

        };

        public static void Initialize(IServiceProvider serviceProvider) {
            using (var dbContext = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null)) {

                PopulateTestData(dbContext);


            }
        }

        public static void PopulateTestData(AppDbContext dbContext) {

            foreach (var item in dbContext.Users) {
                dbContext.Remove(item);
            }

            dbContext.SaveChanges();

            Users.Add(User1);
            Users.Add(User2);
            Users.Add(User3);

            dbContext.Users.AddRange(Users);

            dbContext.SaveChanges();
        }
    }
}