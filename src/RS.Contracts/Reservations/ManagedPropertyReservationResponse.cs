using RS.Domain.Enums;


public record ManagedPropertyReservationResponse(
    Guid ReservationId,
    Guid PropertyId,
    string PropertyTitle,
    string BuyerFullName
    , string BuyerEmail,
    string BuyerPhoneNumber,
    decimal ReservationFee,
    ReservationStatus Status,
    DateTime ReservedAt,
   DateTime ExpiresAt
    );

