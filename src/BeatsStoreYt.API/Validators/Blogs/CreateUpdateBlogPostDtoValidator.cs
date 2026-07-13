using BeatsStoreYt.API.DTOs.Blogs;
using BeatsStoreYt.API.Data;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BeatsStoreYt.API.Validators.Blogs;

public class CreateUpdateBlogPostDtoValidator : AbstractValidator<CreateUpdateBlogPostDto>
{
    private readonly BeatsStoreDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateUpdateBlogPostDtoValidator(BeatsStoreDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;

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
            .WithMessage("Slug חייב להכיל אותיות קטנות באנגלית, מספרים ומקפים בלבד")
            .MustAsync(async (dto, slug, cancellation) =>
            {
                var currentPostId = GetCurrentPostIdFromRoute();
                var exists = await _dbContext.BlogPosts.AnyAsync(
                    p => p.Slug == slug && (!currentPostId.HasValue || p.Id != currentPostId.Value),
                    cancellation);

                return !exists;
            })
            .WithMessage("Slug already exists. Please choose a unique one.");
    }

    private int? GetCurrentPostIdFromRoute()
    {
        var routeValues = _httpContextAccessor.HttpContext?.Request?.RouteValues;
        if (routeValues is null)
            return null;

        if (routeValues.TryGetValue("id", out var value) && value is not null && int.TryParse(value.ToString(), out var id))
            return id;

        return null;
    }
}
