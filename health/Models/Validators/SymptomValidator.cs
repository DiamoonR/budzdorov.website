using health.Data;
using FluentValidation;

namespace health.Models.Validators
{
    public class SymptomValidator : AbstractValidator<Symptom>
    {
        SiteContext db;
        public SymptomValidator(SiteContext context)
        {
            db = context;
            //RuleFor(s => s.Content).NotEmpty().WithMessage("Не заполнен текст");
        }
    }
}
