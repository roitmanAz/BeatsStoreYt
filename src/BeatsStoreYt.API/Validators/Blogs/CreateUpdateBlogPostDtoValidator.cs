using BeatsStoreYt.API.DTOs.Blogs;
using FluentValidation;

namespace BeatsStoreYt.API.Validators.Blogs;

public class CreateUpdateBlogPostDtoValidator : AbstractValidator<CreateUpdateBlogPostDto>
{
    public CreateUpdateBlogPostDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("כותרת היא שדה חובה")
            .MaximumLength(200).WithMessage("כותרת לא יכולה להיות ארוכה מ-200 תווים");

        RuleFor(x => x.Subtitle)
            .NotEmpty().WithMessage("כותרת משנית היא שדה חובה")
            .MaximumLength(500).WithMessage("כותרת משנית לא יכולה להיות ארוכה מ-500 תווים");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("תוכן הוא שדה חובה");

        RuleFor(x => x.CoverImageUrl)
            .NotEmpty().WithMessage("תמונת כיסוי היא שדה חובה")
            .MaximumLength(1000).WithMessage("כתובת תמונה ארוכה מדי")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("כתובת תמונה לא תקינה");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug הוא שדה חובה")
            .MaximumLength(250).WithMessage("Slug ארוך מדי")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug חייב להכיל אותיות קטנות באנגלית, מספרים ומקפים בלבד");
    }
}
