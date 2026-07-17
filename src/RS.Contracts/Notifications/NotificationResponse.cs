namespace RS.Contracts.Notifications;

public record NotificationResponse(
    Guid Id,
    string Title,
    string Message,
    bool IsRead,
    DateTime CreatedAt
);
