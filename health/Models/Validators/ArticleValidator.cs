using System.Linq;
using health.Data;
using FluentValidation;

namespace health.Models.Validators
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        SiteContext db;
        public ArticleValidator(SiteContext context)
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
            return db.Articles.FirstOrDefault(x => x.IdName == idname) == null;
        }
    }
}