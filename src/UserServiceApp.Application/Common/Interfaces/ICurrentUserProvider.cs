using UserServiceApp.Application.Common.Models;

namespace UserServiceApp.Application.Common.Interfaces;

public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}