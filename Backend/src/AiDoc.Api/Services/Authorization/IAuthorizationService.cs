using System.Threading;
using System.Threading.Tasks;
using AiDoc.Api.Services.Authorization.Dtos;

namespace AiDoc.Api.Services.Authorization;

public interface IAuthorizationService
{
    Task<AuthorizationResponse> RegisterNewUserAsync(RegisterNewUserRequest request, CancellationToken cancellationToken);
    
    Task<AuthorizationResponse> AuthorizeUserAsync(AuthorizeUserRequest request, CancellationToken cancellationToken);
}