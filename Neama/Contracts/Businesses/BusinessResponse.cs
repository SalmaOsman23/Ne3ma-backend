namespace Ne3ma.Contracts.Businesses;

public record BusinessResponse(
    Guid Id,
    string Name,
    string Address,
    string Phone,
    string Description,
    bool IsApproved
);
