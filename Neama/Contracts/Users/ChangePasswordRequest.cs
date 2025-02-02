namespace Ne3ma.Contracts.Users;

public record ChangePasswordRequest
(
    string CurrentPassword,
    string NewPassword
);