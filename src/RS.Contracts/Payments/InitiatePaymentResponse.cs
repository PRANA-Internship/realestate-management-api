namespace RS.Contracts.Payments;

public record InitializePaymentResponse(
    string CheckoutUrl,
    string TxRef
);
