using FluentValidation;
using KooliProjekt.Application.Data;
using System.Linq;

namespace KooliProjekt.Application.Features.Projects
{
    public class SaveProjectCommandValidator : AbstractValidator<SaveProjectCommand>
    {
        public SaveProjectCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Project name is required")
                .MaximumLength(50).WithMessage("Project name cannot exceed 50 characters")
                .Custom((name, validationContext) =>
                {
                    var command = validationContext.InstanceToValidate;

                    // Näide: kontrollime, et nimi ei oleks duplikaat
                    var exists = context.Projects.Any(p => p.Name == name && p.Id != command.Id);
                    if (exists)
                    {
                        validationContext.AddFailure("Name", "A project with this name already exists");
                    }
                });

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required");

            RuleFor(x => x.Deadline)
                .NotEmpty().WithMessage("Deadline is required")
                .GreaterThan(x => x.StartDate).WithMessage("Deadline must be after start date");

            RuleFor(x => x.Budget)
                .GreaterThanOrEqualTo(0).WithMessage("Budget must be zero or positive");

            RuleFor(x => x.HourlyRate)
                .GreaterThanOrEqualTo(0).WithMessage("Hourly rate must be zero or positive");
        }
    }
}
