﻿namespace Ne3ma.Contracts.Authentication;

public record AuthResponse(
    string Id,
    string? Email,
    string FirstName,
    string LastName,
    string Token,
    int ExpireIn,
    string RefreshToken,
    DateTime RefreshTokenExpiration
);
