using BeatsStoreYt.API.DTOs.Reviews;
using FluentValidation;

namespace BeatsStoreYt.API.Validators.Reviews;

public class CreateSiteReviewRequestDtoValidator : AbstractValidator<CreateSiteReviewRequestDto>
{
    public CreateSiteReviewRequestDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("שם פרטי הוא שדה חובה")
            .MaximumLength(100).WithMessage("שם פרטי לא יכול להיות ארוך מ-100 תווים");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("שם משפחה הוא שדה חובה")
            .MaximumLength(100).WithMessage("שם משפחה לא יכול להיות ארוך מ-100 תווים");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("תבנית מייל לא תקינה")
            .MaximumLength(256).WithMessage("מייל לא יכול להיות ארוך מ-256 תווים");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("תוכן הוא שדה חובה")
            .MaximumLength(2000).WithMessage("תוכן לא יכול להיות ארוך מ-2000 תווים");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("דירוג חייב להיות בין 1 ל-5");
    }
}
