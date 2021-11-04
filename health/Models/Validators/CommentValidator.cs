using health.Data;
using FluentValidation;

namespace health.Models.Validators
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        SiteContext db;
        public CommentValidator(SiteContext context)
        {
            db = context;
            RuleFor(s => s.Message).NotEmpty().WithMessage("Пустой комментарий");
            RuleFor(s => s.UserName).NotEmpty().WithMessage("Не имя");
        }
    }
}
