using BeatsStoreYt.API.DTOs.Auth;
using FluentValidation;

namespace BeatsStoreYt.API.Validators.Auth;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("מייל הוא שדה חובה")
            .EmailAddress().WithMessage("תבנית מייל לא תקינה")
            .MaximumLength(256).WithMessage("מייל לא יכול להיות ארוך מ-256 תווים");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("סיסמה היא שדה חובה");
    }
}
