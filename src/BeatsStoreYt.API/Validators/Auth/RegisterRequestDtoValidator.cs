using BeatsStoreYt.API.DTOs.Auth;
using FluentValidation;

namespace BeatsStoreYt.API.Validators.Auth;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("שם פרטי הוא שדה חובה")
            .MaximumLength(100).WithMessage("שם פרטי לא יכול להיות ארוך מ-100 תווים");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("שם משפחה הוא שדה חובה")
            .MaximumLength(100).WithMessage("שם משפחה לא יכול להיות ארוך מ-100 תווים");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("מספר פלאפון לא יכול להיות ארוך מ-20 תווים")
            .Matches(@"^[0-9+\-\s()]*$").When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("מספר פלאפון מכיל תווים לא חוקיים");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("מייל הוא שדה חובה")
            .EmailAddress().WithMessage("תבנית מייל לא תקינה")
            .MaximumLength(256).WithMessage("מייל לא יכול להיות ארוך מ-256 תווים");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("סיסמה היא שדה חובה")
            .MinimumLength(8).WithMessage("סיסמה חייבת להכיל לפחות 8 תווים")
            .Matches("[A-Za-z]").WithMessage("סיסמה חייבת להכיל לפחות אות אחת")
            .Matches("\\d").WithMessage("סיסמה חייבת להכיל לפחות ספרה אחת");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("אימות סיסמה לא תואם לסיסמה");
    }
}
