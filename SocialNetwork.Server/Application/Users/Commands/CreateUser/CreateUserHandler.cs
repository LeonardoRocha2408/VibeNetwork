using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Server.Domain.Entities;
using SocialNetwork.Server.Infrastructure.Common.Exceptions;
using SocialNetwork.Server.Infrastructure.Persistence;
using SocialNetwork.Server.Infrastructure.Services;

namespace SocialNetwork.Server.Application.Users.Commands.CreateUser;
public class CreateUserHandler(AppDbContext appDbContext, PasswordHashService passwordHashService) : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly PasswordHashService _passwordHashService = passwordHashService;
    public async Task<Guid> Handle(CreateUserCommand command, CancellationToken ct)
    {
        var emailAlreadyExists = await _appDbContext.Users.AnyAsync(u =>  u.Email == command.Email, ct);

        if (emailAlreadyExists) throw new ConflictException(Errors.EmailAlreadyExists, "Email already exist");

        var user = new UserScaffold(command.Email, command.Username, _passwordHashService.Hash(command.Password));

        await _appDbContext.Users.AddAsync(user, ct);
        await _appDbContext.SaveChangesAsync(ct);

        return user.Id;
    }
}
