namespace UserServiceApp.Tests.Shared.Extensions;

public static class RequestHeaderExtensions
{
    public static void AddAuthorizationHeader(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    public static void AddNewUserCultureHeader(this HttpClient client, string newUserCulture)
    {
        client.DefaultRequestHeaders.Add("NewUserCulture", newUserCulture);
    }
}
