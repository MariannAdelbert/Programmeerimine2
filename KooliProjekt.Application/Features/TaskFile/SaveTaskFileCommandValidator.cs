using FluentValidation;
using System;

namespace KooliProjekt.Application.Features.TaskFiles
{
    public class SaveTaskFileCommandValidator : AbstractValidator<SaveTaskFileCommand>
    {
        public SaveTaskFileCommandValidator()
        {
            RuleFor(x => x.TaskId)
                .GreaterThan(0)
                .WithMessage("TaskId peab olema suurem kui 0.");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("FileName on kohustuslik.")
                .MaximumLength(100).WithMessage("FileName võib olla maksimaalselt 100 tähemärki.");

            RuleFor(x => x.FilePath)
                .NotEmpty().WithMessage("FilePath on kohustuslik.")
                .MaximumLength(100).WithMessage("FilePath võib olla maksimaalselt 100 tähemärki.");

            RuleFor(x => x.UploadDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("UploadDate ei saa olla tulevikus.");
        }
    }
}