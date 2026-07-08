using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RS.Domain.Entities;

namespace RS.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TxRef)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.TxRef)
            .IsUnique();

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Currency)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.ChapaReference)
            .HasMaxLength(100);

        builder.HasOne(x => x.Reservation)
            .WithMany()
            .HasForeignKey(x => x.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
