using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RS.Domain.Entities;

namespace RS.Infrastructure.Persistence.Configurations;

public class ReservationConfiguration
    : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.BuyerFullName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.BuyerEmail)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.BuyerPhoneNumber)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.ReservationFee)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasConversion<string>();

        builder.HasOne(x => x.Property)
            .WithMany()
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
