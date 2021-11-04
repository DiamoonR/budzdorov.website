using health.Data;
using FluentValidation;

namespace health.Models.Validators
{
    public class RecipeValidator : AbstractValidator<Recipe>
    {
        SiteContext db;
        public RecipeValidator(SiteContext context)
        {
            db = context;
            RuleFor(s => s.Content).NotEmpty().WithMessage("Не заполнен текст");
        }
    }
}
