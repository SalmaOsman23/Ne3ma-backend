namespace Ne3ma.Contracts.Authentication;

public record LoginRequest(
    string Email,
    string Password
);