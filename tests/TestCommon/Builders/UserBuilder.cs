using TestCommon.Common.Interfaces;
using UserServiceApp.Domain.UsersAggregate;

namespace TestCommon.Builders;
public class UserBuilder : IBuilder<User>
{
    private Guid _id = Guid.NewGuid();
    private string _username = "defaultUsername";
    private string _fullName = "Default User";
    private string _email = "user@example.com";
    private string _mobileNumber = "000-000-0000";
    private string _language = "English";
    private string _culture = "en-US";
    private string _passwordHash = "hashedPassword"; // Note: Ensure to use a realistically hashed password for actual tests
    private bool IsAdmin = false;

    public User Build()
    {
        _id = Guid.NewGuid();
        return new User(_username, _fullName, _email, _mobileNumber, _language, _culture, _passwordHash, IsAdmin, _id);
    }

    public UserBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public UserBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }

    public UserBuilder WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithMobileNumber(string mobileNumber)
    {
        _mobileNumber = mobileNumber;
        return this;
    }

    public UserBuilder WithLanguage(string language)
    {
        _language = language;
        return this;
    }

    public UserBuilder WithCulture(string culture)
    {
        _culture = culture;
        return this;
    }

    public UserBuilder WithPasswordHash(string passwordHash)
    {
        _passwordHash = passwordHash;
        return this;
    }

    public UserBuilder WithIsAdmin(bool isAdmin)
    {
        IsAdmin = isAdmin;
        return this;
    }
}
