using FluentValidation;

namespace KooliProjekt.Application.Features.ProjectTasks
{
    public class SaveProjectTaskCommandValidator : AbstractValidator<SaveProjectTaskCommand>
    {
        public SaveProjectTaskCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ProjectId).GreaterThan(0);
            RuleFor(x => x.ResponsibleUserId).GreaterThan(0);
        }
    }
}