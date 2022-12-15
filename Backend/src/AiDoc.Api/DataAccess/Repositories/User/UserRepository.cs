using System.Threading;
using System.Threading.Tasks;
using AiDoc.Api.DataAccess.Repositories.User.Dtos;
using AiDoc.Platform.Data.Factories;
using AiDoc.Platform.Data.Providers;
using Dapper;

namespace AiDoc.Api.DataAccess.Repositories.User;

public sealed class UserRepository : IUserRepository
{
    private readonly IPostgresConnectionFactory _factory;
    private readonly IDbTransactionsProvider _transactionsProvider;

    public UserRepository(IPostgresConnectionFactory factory, IDbTransactionsProvider transactionsProvider)
    {
        _factory = factory;
        _transactionsProvider = transactionsProvider;
    }

    public async Task InsertUserAsync(InsertUserDbCmd cmd, CancellationToken cancellationToken)
    {
        const string query = @"insert into users  
                               (id, password, email, profile_pic_url, full_name) 
                               values(:Id::uuid, :PasswordHash, :Email, :ProfilePicUrl, :FullName);";

        var connection = _factory.GetConnection();
        var param = new
        {
            cmd.Id,
            cmd.PasswordHash,
            cmd.Email,
            cmd.ProfilePicUrl,
            cmd.FullName
        };
        await connection.ExecuteAsync(query, param, _transactionsProvider.Current, commandTimeout: 30);
    }

    public async Task<UserDb?> SelectUserAsync(SelectUserDbCmd cmd, CancellationToken cancellationToken)
    {
        const string query = @"select * from users where id = :Id;";

        var connection = _factory.GetConnection();
        var param = new {cmd.Id};
        return await connection.QueryFirstOrDefaultAsync<UserDb>(
            query,
            param,
            _transactionsProvider.Current,
            commandTimeout: 30);
    }

    public async Task<UserDb?> SelectUserByEmailAsync(SelectUserByEmailDbCmd cmd, CancellationToken cancellationToken)
    {
        const string query = @"select * from users where email = :Email;";

        var connection = _factory.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<UserDb>(
            query,
            cmd,
            _transactionsProvider.Current,
            commandTimeout: 30);
    }
}