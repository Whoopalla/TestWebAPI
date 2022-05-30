using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestWebAPI.Core.UserAggregate;

namespace TestWebAPI.Infrastructure.Data.Config {
    internal class UserConfiguration : IEntityTypeConfiguration<User> {
        public void Configure(EntityTypeBuilder<User> builder) {
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(e => e.Login).
                IsUnique();
        }
    }
}
