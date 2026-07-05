namespace RS.Contracts.Reservations;

public record ReservationResponse(
    Guid Id,
    Guid PropertyId,
    string PropertyTitle,
    decimal PropertyPrice,
    decimal ReservationFee,
    string Status,
    DateTime ReservedAt,
    DateTime ExpiresAt);
