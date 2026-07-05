using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RS.Domain.Entities;

namespace RS.Infrastructure.Persistence.Configurations;

public class SystemConfigurationConfiguration
    : IEntityTypeConfiguration<SystemConfiguration>
{
    public void Configure(EntityTypeBuilder<SystemConfiguration> builder)
    {
        builder.ToTable("SystemConfigurations");

        builder.HasKey(configuration => configuration.Id);

        builder.Property(configuration => configuration.Key)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(configuration => configuration.Key)
            .IsUnique();

        builder.Property(configuration => configuration.Value)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(configuration => configuration.Description)
            .HasMaxLength(500);

        builder.Property(configuration => configuration.CreatedAt)
            .IsRequired();

        builder.Property(configuration => configuration.UpdatedAt);
    }
}
