using RS.Domain.Enums;

namespace RS.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }

    public Guid PropertyId { get; set; }

    public Guid? BuyerUserId { get; set; }

    public string BuyerFullName { get; set; } = default!;

    public string BuyerEmail { get; set; } = default!;

    public string BuyerPhoneNumber { get; set; } = default!;

    public decimal ReservationFee { get; set; }

    public ReservationStatus Status { get; set; }

    public DateTime ReservedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public Property Property { get; set; } = default!;
}
