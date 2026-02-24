using FluentValidation;
using System;

namespace KooliProjekt.Application.Features.WorkLogs
{
    public class SaveWorkLogCommandValidator : AbstractValidator<SaveWorkLogCommand>
    {
        public SaveWorkLogCommandValidator()
        {
            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("TaskId peab olema suurem kui 0.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId peab olema suurem kui 0.");

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Date ei saa olla tulevikus.");

            RuleFor(x => x.HoursSpent)
                .GreaterThan(0).WithMessage("HoursSpent peab olema suurem kui 0.")
                .LessThanOrEqualTo(24).WithMessage("HoursSpent ei tohi ületada 24 tundi.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description on kohustuslik.")
                .MaximumLength(100).WithMessage("Description võib olla maksimaalselt 100 tähemärki.");
        }
    }
}