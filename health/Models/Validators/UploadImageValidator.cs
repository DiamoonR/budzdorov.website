using FluentValidation;

namespace health.Models.Validators
{
    public class UploadImageValidator : AbstractValidator<UploadImage>
    {
        public UploadImageValidator()
        {
            RuleFor(s => s.FileName).NotEmpty().WithMessage("Не заполнено имя файла");
        }
    }
}
