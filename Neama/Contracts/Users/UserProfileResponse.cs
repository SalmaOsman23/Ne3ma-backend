﻿namespace Ne3ma.Contracts.Users;

public record UserProfileResponse(
    string Email,
    string UserName,
    string FirstName,
    string LastName,

    int orders
);
