using System;

namespace Ne3ma.Contracts.FoodItems;

public class FoodItemRequestValidator : AbstractValidator<FoodItemRequest>
{
    public FoodItemRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Food item name is required.")
            .MaximumLength(100)
            .WithMessage("Food item name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");

        //RuleFor(x => x.ImageUrl)
            //.NotEmpty()
            //.WithMessage("Image URL is required.")
            //.Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            //.WithMessage("Invalid image URL format.");

        RuleFor(x => x.QuantityAvailable)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Quantity must be at least 1.");

        RuleFor(x => x.ExpiryTime)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Expiry time must be in the future.");
    }

}
