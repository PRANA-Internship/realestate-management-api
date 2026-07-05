namespace RS.Contracts.Reservations;

public record ReservationDetailResponse(
    Guid Id,
    Guid PropertyId,
    string PropertyTitle,
    decimal PropertyPrice,

    string BuyerFullName,
    string BuyerEmail,
    string BuyerPhoneNumber,

    decimal ReservationFee,
    string Status,

    DateTime ReservedAt,
    DateTime ExpiresAt);
