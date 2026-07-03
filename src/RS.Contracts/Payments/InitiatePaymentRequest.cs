namespace RS.Contracts.Payments;

public record InitializePaymentRequest(
    decimal Amount,
    string Currency,
    string Email,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    Guid? RelatedEntityId,
    string? RelatedEntityType
);
