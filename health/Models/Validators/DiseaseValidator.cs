using System.Linq;
using health.Data;
using FluentValidation;

namespace health.Models.Validators
{
    public class DiseaseValidator : AbstractValidator<Disease>
    {
        SiteContext db;
        public DiseaseValidator(SiteContext context)
        {
            db = context;
            RuleSet("Add", () => {
                RuleFor(s => s.Name).NotEmpty().WithMessage("Не заполнено название");
                RuleFor(s => s.IdName).Must(BeUniqueIdName).WithMessage("Такой идентификатор уже есть");
            });
            RuleSet("Edit", () => {
                RuleFor(s => s.Name).NotEmpty().WithMessage("Не заполнено название");
            });
        }

        private bool BeUniqueIdName(string idname)
        {
            return db.Pages.FirstOrDefault(x => x.IdName == idname) == null;
        }
    }
}