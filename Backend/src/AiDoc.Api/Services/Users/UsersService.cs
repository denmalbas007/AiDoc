using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AiDoc.Api.DataAccess.Repositories.User;
using AiDoc.Api.DataAccess.Repositories.User.Dtos;
using AiDoc.Api.Services.Users.Dtos;
using AiDoc.Platform.Exceptions;
using Microsoft.AspNetCore.Http;

namespace AiDoc.Api.Services.Users;

public sealed class UsersService : IUsersService
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public UsersService(IHttpContextAccessor contextAccessor, IUserRepository userRepository)
    {
        _contextAccessor = contextAccessor;
        _userRepository = userRepository;
    }

    public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(
            _contextAccessor.HttpContext?.User.Identities.FirstOrDefault()?.Claims.First(x => x.Type == "Id").Value!);
        var user = await _userRepository.SelectUserAsync(new SelectUserDbCmd(userId), cancellationToken);
        if (user is null)
            throw new ExceptionWithCode(404, "Can't found user");

        return new User(userId, user.Email, user.FullName);
    }
}