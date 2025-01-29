namespace Ne3ma.Contracts.Authentication;

public record RefreshTokenRequest(
    string Token,
    string RefreshToken
);
