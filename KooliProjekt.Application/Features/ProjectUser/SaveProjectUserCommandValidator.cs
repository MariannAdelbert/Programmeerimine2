using FluentValidation;

namespace KooliProjekt.Application.Features.ProjectUsers
{
    public class SaveProjectUserCommandValidator : AbstractValidator<SaveProjectUserCommand>
    {
        public SaveProjectUserCommandValidator()
        {
            RuleFor(x => x.ProjectId).GreaterThan(0).WithMessage("ProjectId peab olema suurem kui 0.");
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId peab olema suurem kui 0.");
            RuleFor(x => x.RoleInProject).NotEmpty().MaximumLength(50).WithMessage("RoleInProject on kohustuslik ja max 50 tähemärki.");
        }
    }
}