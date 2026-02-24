using FluentValidation;

namespace KooliProjekt.Application.Features.User
{
    public class SaveUserCommandValidator : AbstractValidator<SaveUserCommand>
    {
        public SaveUserCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName on kohustuslik.")
                .MaximumLength(50).WithMessage("UserName võib olla maksimaalselt 50 tähemärki.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name on kohustuslik.")
                .MaximumLength(100).WithMessage("Name võib olla maksimaalselt 100 tähemärki.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email on kohustuslik.")
                .MaximumLength(100).WithMessage("Email võib olla maksimaalselt 100 tähemärki.")
                .EmailAddress().WithMessage("Email peab olema korrektne.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password on kohustuslik.")
                .MinimumLength(6).WithMessage("Password peab olema vähemalt 6 tähemärki.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role on kohustuslik.")
                .MaximumLength(50).WithMessage("Role võib olla maksimaalselt 50 tähemärki.");
        }
    }
}