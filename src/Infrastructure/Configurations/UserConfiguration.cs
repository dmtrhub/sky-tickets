using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.DateOfBirth)
            .HasColumnType("date");

        builder.Property(u => u.Role)
            .HasConversion<string>();

        builder.Property(u => u.Gender)
            .HasConversion<string>();
    }
}
