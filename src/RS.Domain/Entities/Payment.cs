using System;

namespace RS.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }

    public Guid ReservationId { get; set; }

    public string TxRef { get; set; } = default!;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "ETB";

    public string Status { get; set; } = "Pending"; // Pending, Completed, Failed

    public string? ChapaReference { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Reservation Reservation { get; set; } = default!;
}
