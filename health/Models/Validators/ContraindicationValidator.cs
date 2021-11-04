using health.Data;
using FluentValidation;

namespace health.Models.Validators
{
    public class ContraindicationValidator : AbstractValidator<Contraindication>
    {
        SiteContext db;
        public ContraindicationValidator(SiteContext context)
        {
            db = context;
        }
    }
}
