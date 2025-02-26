namespace Ne3ma.Contracts.Businesses;

public record BusinessRequest(
    string Name,
    string Address,
    double Latitude,
    double Longitude,
    string Phone,
    string Description
);