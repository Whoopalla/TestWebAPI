using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestWebAPI.Infrastructure.Data;

namespace TestWebAPI.Infrastructure {
    public static class StartupSetup {
        public static void AddDbContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
    }
}