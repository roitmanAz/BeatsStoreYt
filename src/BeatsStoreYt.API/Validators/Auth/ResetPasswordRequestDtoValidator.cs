using BeatsStoreYt.API.DTOs.Auth;
using FluentValidation;

namespace BeatsStoreYt.API.Validators.Auth;

public class ResetPasswordRequestDtoValidator : AbstractValidator<ResetPasswordRequestDto>
{
    public ResetPasswordRequestDtoValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("טוקן הוא שדה חובה");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("סיסמה חדשה היא שדה חובה")
            .MinimumLength(8).WithMessage("סיסמה חייבת להכיל לפחות 8 תווים")
            .Matches("[A-Za-z]").WithMessage("סיסמה חייבת להכיל לפחות אות אחת")
            .Matches("\\d").WithMessage("סיסמה חייבת להכיל לפחות ספרה אחת");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword).WithMessage("אימות סיסמה לא תואם לסיסמה החדשה");
    }
}
