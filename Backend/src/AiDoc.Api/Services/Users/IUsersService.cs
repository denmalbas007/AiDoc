using System.Threading;
using System.Threading.Tasks;
using AiDoc.Api.Services.Users.Dtos;

namespace AiDoc.Api.Services.Users;

public interface IUsersService
{
    public Task<User> GetCurrentUserAsync(CancellationToken cancellationToken);
    public Task<UserWithContent> GetCurrentUserContentAsync(CancellationToken cancellationToken);
}