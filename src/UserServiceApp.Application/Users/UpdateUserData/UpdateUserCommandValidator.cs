using FluentValidation;
using UserServiceApp.Application.Common.Interfaces;

namespace UserServiceApp.Application.Users.UpdateUserData;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private IUsersRepository _usersRepository;

    public UpdateUserCommandValidator(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(UsernameIsUniqueAsync)
            .WithMessage("Username already exists");

        RuleFor(x => x.FullName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(EmailIsUniqueAsync)
            .WithMessage("Email already exists");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage("Password must have at least one number, symbol and capital letter. The minumum lenght is 8 characters,");

        RuleFor(x => x.MobileNumber)
            .NotEmpty()
            .Matches(@"^\+[1-9]\d{1,14}$")
            .WithMessage("Mobile number must be in international format. Example: +40721234567");

        RuleFor(x => x.NewCulture)
            .NotEmpty()
            .Matches(@"^[A-Za-z]{1,8}(-[A-Za-z0-9]{1,8})*$")
            .WithMessage("Culture should have correct format. Example: en-US");
    }

    private async Task<bool> UsernameIsUniqueAsync(UpdateUserCommand command, string username, CancellationToken cancellationToken)
    {
        var usernameIsUnique = await _usersRepository.UsernameIsUniqueAsync(command.UserId, username, cancellationToken);
        return usernameIsUnique;
    }

    private async Task<bool> EmailIsUniqueAsync(UpdateUserCommand command, string email, CancellationToken cancellationToken)
    {
        var emailIsUnique = await _usersRepository.EmailIsUniqueAsync(command.UserId, email, cancellationToken);
        return emailIsUnique;
    }
}
