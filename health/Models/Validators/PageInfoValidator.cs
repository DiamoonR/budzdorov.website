using health.Data;
using FluentValidation;

namespace health.Models.Validators
{
    public class PageInfoValidator : AbstractValidator<PageInfo>
    {
        SiteContext db;
        public PageInfoValidator(SiteContext context)
        {
            db = context;
            RuleFor(s => s.Url).NotEmpty().WithMessage("Не заполнен Url");
        }
    }
}
