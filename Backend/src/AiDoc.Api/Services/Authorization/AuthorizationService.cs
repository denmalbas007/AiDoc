using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AiDoc.Api.DataAccess.Repositories.User;
using AiDoc.Api.DataAccess.Repositories.User.Dtos;
using AiDoc.Api.Services.Authorization.Dtos;
using AiDoc.Platform.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AiDoc.Api.Services.Authorization;

public sealed class AuthorizationService : IAuthorizationService
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConfiguration _configuration;

    public AuthorizationService(
        IHttpContextAccessor contextAccessor,
        IUserRepository userRepository,
        IConfiguration configuration)
    {
        _contextAccessor = contextAccessor;
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<AuthorizationResponse> RegisterNewUserAsync(
        RegisterNewUserRequest request,
        CancellationToken cancellationToken)
    {
        var getCmd = new SelectUserByEmailDbCmd(request.Email);
        var user = await _userRepository.SelectUserByEmailAsync(getCmd, cancellationToken);
        if (user is not null)
            throw new ExceptionWithCode(403, "User already exists");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var id = Guid.NewGuid();
        var insertCmd = new InsertUserDbCmd(
            id,
            request.Email,
            request.FullName,
            hashedPassword,
            request.ProfilePicUrl);
        await _userRepository.InsertUserAsync(insertCmd, cancellationToken);
        var key = Encoding.ASCII.GetBytes(
            _configuration["JWT_KEY"] ?? _configuration["Jwt:Key"] ?? throw new ArgumentNullException());
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new[]
                {
                    new Claim("Id", id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, request.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return new AuthorizationResponse(jwtToken);
    }

    public async Task<AuthorizationResponse> AuthorizeUserAsync(
        AuthorizeUserRequest request,
        CancellationToken cancellationToken)
    {
        var getCmd = new SelectUserByEmailDbCmd(request.Email);
        var user = await _userRepository.SelectUserByEmailAsync(getCmd, cancellationToken);
        if (user is null)
            throw new ExceptionWithCode(404, "User not found");
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new ExceptionWithCode(403, "Incorrect password");
        var key = Encoding.ASCII.GetBytes(
            _configuration["JWT_KEY"] ?? _configuration["Jwt:Key"] ?? throw new ArgumentNullException());
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, request.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return new AuthorizationResponse(jwtToken);
    }
}