using FluentValidation;

namespace UserServiceApp.Application.Users.GetUserData;
public class GetUserDataQueryValidator : AbstractValidator<GetUserDataQuery>
{
    public GetUserDataQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
