using BeatsStoreYt.API.DTOs.Services;
using FluentValidation;

namespace BeatsStoreYt.API.Validators.Services;

public class AddCustomStyleCommentRequestDtoValidator : AbstractValidator<AddCustomStyleCommentRequestDto>
{
    public AddCustomStyleCommentRequestDtoValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("תוכן הערה הוא שדה חובה")
            .MaximumLength(2000).WithMessage("תוכן הערה לא יכול להיות ארוך מ-2000 תווים");
    }
}
