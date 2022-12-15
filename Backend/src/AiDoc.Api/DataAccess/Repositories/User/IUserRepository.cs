using System.Threading;
using System.Threading.Tasks;
using AiDoc.Api.DataAccess.Repositories.Abstractions;
using AiDoc.Api.DataAccess.Repositories.User.Dtos;

namespace AiDoc.Api.DataAccess.Repositories.User;

public interface IUserRepository : IRepository
{
    Task InsertUserAsync(InsertUserDbCmd cmd, CancellationToken cancellationToken);
    Task<UserDb?> SelectUserAsync(SelectUserDbCmd cmd, CancellationToken cancellationToken);
    Task<UserDb?> SelectUserByEmailAsync(SelectUserByEmailDbCmd cmd, CancellationToken cancellationToken);
}