namespace Ne3ma.Errors;

public static class BusinessErrors
{
    public static readonly Error NotFound =
        new("Business.NotFound", "The requested business was not found.", StatusCodes.Status404NotFound);

    public static readonly Error Unauthorized =
        new("Business.Unauthorized", "You are not authorized to perform this action.", StatusCodes.Status403Forbidden);

    public static readonly Error AlreadyApproved =
        new("Business.AlreadyApproved", "This business has already been approved.", StatusCodes.Status400BadRequest);

    public static readonly Error ValidationFailed =
        new("Business.ValidationFailed", "Invalid business data provided.", StatusCodes.Status400BadRequest);
}
