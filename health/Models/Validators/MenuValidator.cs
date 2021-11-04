using health.Data;
using FluentValidation;

namespace health.Models.Validators
{
    public class MenuValidator: AbstractValidator<Menu>
    {
        SiteContext db;
        public MenuValidator(SiteContext context)
        {
            db = context;
            RuleFor(s => s.Name).NotEmpty().WithMessage("Не заполнено название");
            RuleFor(s => s.Url).NotEmpty().WithMessage("Не заполнено поле ссылки");
        }
    }
}
