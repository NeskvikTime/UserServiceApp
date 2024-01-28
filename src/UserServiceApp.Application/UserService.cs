using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Contracts.Common;

namespace UserServiceApp.Application;
internal class UserService(IUsersRepository _userRepository,
    IUnitOfWork _unitOfWork,
    IJwtTokenGenerator _jwtTokenGenerator) : IUserService
{
    public async Task DeleteUeserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException($"User with id {userId} not found");
        }

        await _userRepository.DeleteAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<AuthenticationResult> GetUserDataAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        var result = new AuthenticationResult(user, token);

        return result;
    }
}
