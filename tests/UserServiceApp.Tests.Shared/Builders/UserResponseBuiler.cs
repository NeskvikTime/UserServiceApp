using UserServiceApp.Tests.Shared.Common.Interfaces;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.Tests.Shared.Builders;

public class UserResponseBuilder : IBuilder<UserResponse>
{
    private Guid _id = Guid.NewGuid();
    private string _username = "defaultUsername";
    private string _fullName = "Default User";
    private string _email = "user@example.com";
    private string _mobileNumber = "000-000-0000";
    private string _language = "English";
    private string _culture = "en-US";
    private bool _isAdmin = false;

    public UserResponseBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public UserResponseBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }

    public UserResponseBuilder WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public UserResponseBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserResponseBuilder WithMobileNumber(string mobileNumber)
    {
        _mobileNumber = mobileNumber;
        return this;
    }

    public UserResponseBuilder WithLanguage(string language)
    {
        _language = language;
        return this;
    }

    public UserResponseBuilder WithCulture(string culture)
    {
        _culture = culture;
        return this;
    }

    public UserResponseBuilder IsAdmin(bool isAdmin)
    {
        _isAdmin = isAdmin;
        return this;
    }

    public UserResponse Build()
    {
        return new UserResponse(
            _id,
            _username,
            _fullName,
            _email,
            _mobileNumber,
            _language,
            _culture,
            _isAdmin);
    }
}

