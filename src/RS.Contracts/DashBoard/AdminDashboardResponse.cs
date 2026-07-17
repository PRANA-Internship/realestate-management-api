namespace RS.Contracts.Dashboard;

public record AdminDashboardResponse(
    UserStatistics Users,
    PropertyStatistics Properties,
    ReservationStatistics Reservations
);


public record UserStatistics(
    int Total,
    int Managers,
    int Sales,
    int Buyers
);


public record PropertyStatistics(
    int Total,
    int Available,
    int Reserved,
    int Sold,
    int Rented,
    int UnderMaintenance,
    int Inactive
);


public record ReservationStatistics(
    int Total,
    int PendingPayment,
    int Reserved,
    int Expired
);
