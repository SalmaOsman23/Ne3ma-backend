namespace Ne3ma.Contracts.Businesses;

public record BusinessRequest(
    string Name,
    string Address,
    double Longitude,
    double Latitude,
    string Phone,
    string Description
);