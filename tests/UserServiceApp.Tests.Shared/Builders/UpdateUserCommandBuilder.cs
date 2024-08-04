using UserServiceApp.Application.Users.UpdateUserData;

namespace UserServiceApp.Tests.Shared.Builders;
public class UpdateUserCommandBuilder
{
    private Guid _userId = Guid.NewGuid();
    private string _userName = "defaultUser";
    private string _fullName = "Default User";
    private string _email = "user@example.com";
    private string _mobileNumber = "+1234567890";
    private string _newCulture = "en-US";
    private bool _isAdmin = false;
    private string? _newPassword = null;

    public UpdateUserCommandBuilder WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public UpdateUserCommandBuilder WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public UpdateUserCommandBuilder WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public UpdateUserCommandBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UpdateUserCommandBuilder WithMobileNumber(string mobileNumber)
    {
        _mobileNumber = mobileNumber;
        return this;
    }

    public UpdateUserCommandBuilder WithNewCulture(string newCulture)
    {
        _newCulture = newCulture;
        return this;
    }

    public UpdateUserCommandBuilder IsAdmin(bool isAdmin)
    {
        _isAdmin = isAdmin;
        return this;
    }

    public UpdateUserCommandBuilder WithNewPassword(string? newPassword)
    {
        _newPassword = newPassword;
        return this;
    }

    public UpdateUserCommand Build()
    {
        return new UpdateUserCommand(
            _userId,
            _userName,
            _fullName,
            _email,
            _mobileNumber,
            _isAdmin,
            _newPassword,
            _newCulture);
    }
}
