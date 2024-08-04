using UserServiceApp.Tests.Shared.Common.Interfaces;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.Tests.Shared.Builders;
public class UpdateUserRequestBuilder : IBuilder<UpdateUserRequest>
{
    private string _userName = "defaultUser";
    private string _fullName = "Default User";
    private string _email = "user@example.com";
    private string _mobileNumber = "000-000-0000";
    private bool _isAdmin = false;
    private string? _newPassword = null;

    public UpdateUserRequestBuilder WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public UpdateUserRequestBuilder WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public UpdateUserRequestBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UpdateUserRequestBuilder WithMobileNumber(string mobileNumber)
    {
        _mobileNumber = mobileNumber;
        return this;
    }

    public UpdateUserRequestBuilder IsAdmin(bool isAdmin)
    {
        _isAdmin = isAdmin;
        return this;
    }

    public UpdateUserRequestBuilder WithNewPassword(string? newPassword)
    {
        _newPassword = newPassword;
        return this;
    }

    public UpdateUserRequest Build()
    {
        return new UpdateUserRequest(
            _userName,
            _fullName,
            _email,
            _mobileNumber,
            _isAdmin,
            _newPassword);
    }
}

