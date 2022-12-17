using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AiDoc.Api.DataAccess.Repositories.User;
using AiDoc.Api.DataAccess.Repositories.User.Dtos;
using AiDoc.Api.Services.Users.Dtos;
using AiDoc.Platform.Data.Factories;
using AiDoc.Platform.Data.Providers;
using AiDoc.Platform.Exceptions;
using Dapper;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Microsoft.AspNetCore.Http;

namespace AiDoc.Api.Services.Users;

public sealed class UsersService : IUsersService
{
    private readonly IUserRepository _userRepository;

    private readonly IHttpContextAccessor _contextAccessor;

    // Грязь
    private readonly IPostgresConnectionFactory _factory;
    private readonly IDbTransactionsProvider _transactionsProvider;

    public UsersService(
        IHttpContextAccessor contextAccessor,
        IUserRepository userRepository,
        IDbTransactionsProvider transactionsProvider,
        IPostgresConnectionFactory factory)
    {
        _contextAccessor = contextAccessor;
        _userRepository = userRepository;
        _transactionsProvider = transactionsProvider;
        _factory = factory;
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

    public async Task<UserWithContent> GetCurrentUserContentAsync(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(
            _contextAccessor.HttpContext?.User.Identities.FirstOrDefault()?.Claims.First(x => x.Type == "Id").Value!);
        var user = await _userRepository.SelectUserAsync(new SelectUserDbCmd(userId), cancellationToken);
        if (user is null)
            throw new ExceptionWithCode(404, "Can't found user");
        
        // Create
        //     .Table("users_contents")
        //     .WithColumn("user_id").AsGuid()
        //     .WithColumn("file_id").AsGuid()
        //     .WithColumn("name").AsString()
        //     .WithColumn("url").AsString()
        //     .WithColumn("prediction").AsString()
        //     .WithColumn("created_at").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime);
        //
        // Create
        //     .Table("contents_sentencies_predictions")
        //     .WithColumn("id").AsInt64().Identity().PrimaryKey()
        //     .WithColumn("prediction").AsString()
        //     .WithColumn("file_id").AsGuid();

        var connection = _factory.GetConnection();
        var query = @"select * from users u
                      inner join users_contents uc on u.id = uc.user_id where u.id = :Id";
        var query2 = @"select * from contents_sentencies_predictions csp where file_id = ANY (:Ids)";

        var result = await connection.QueryAsync<UserContentDb>(
            query,
            new {Id = userId},
            _transactionsProvider.Current,
            30);
        var result2 = await connection.QueryAsync<UserContentDb>(
            query2,
            new {Ids = result.Select(x => x.FileId).ToArray()},
            _transactionsProvider.Current,
            30);

        var docMetas = new List<DocumentMeta>();
        foreach (var res in result)
        {
            var d = new DocumentMeta
            {
                Id = res.FileId,
                Name = res.Name,
                Prediction = res.Prediction,
                Url = res.Url,
                Sentences = result2.Where(z => z.FileId == res.FileId).Select(x => x.Prediction).ToArray()
            };
            docMetas.Add(d);
        }

        return new UserWithContent(userId, user.Email, user.FullName, docMetas.ToArray());
    }
}

public class UserContentDb
{
    public Guid FileId { get; init; }
    public string Name { get; init; } = null!;
    public string Url { get; init; } = null!;
    public string Prediction { get; init; } = null!;
}