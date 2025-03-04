namespace Ne3ma.Contracts.Businesses;

public class BusinessRequestValidator : AbstractValidator<BusinessRequest>
{
    public BusinessRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Address)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180);

            RuleFor(x => x.Phone)
            .Matches(@"^(\+20|0)?1[0-9]{9}$")
            .WithMessage("Invalid Egyptian phone number");


        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

    }
}
