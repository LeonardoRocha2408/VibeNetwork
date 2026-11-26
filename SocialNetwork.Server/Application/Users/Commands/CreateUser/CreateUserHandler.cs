using MediatR;
using Microsoft.EntityFrameworkCore;
using Neo4j.Driver;
using SocialNetwork.Server.Infrastructure.Common.Exceptions;
using SocialNetwork.Server.Infrastructure.Persistence;
using SocialNetwork.Server.Infrastructure.Services;
using SocialNetwork.Server.Scaffolds;

namespace SocialNetwork.Server.Application.Users.Commands.CreateUser;
public class CreateUserHandler(
    AppDbContext appDbContext, 
    PasswordHashService passwordHashService,
    IDriver neo4j) : IRequestHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand command, CancellationToken ct)
    {
        var emailAlreadyExists = await appDbContext.Users.AnyAsync(u =>  u.Email == command.Email, ct);

        if (emailAlreadyExists) throw new ConflictException(Errors.EmailAlreadyExists, "Email already exist");

        var user = new UserScaffold(command.Email, command.Username, passwordHashService.Hash(command.Password));

        await appDbContext.Users.AddAsync(user, ct);
        await appDbContext.SaveChangesAsync(ct);

        var session = neo4j.AsyncSession();

        await session.RunAsync(
            """
            CREATE (u:User {id: $userId})
            """, 
            new
            {
                userId = user.Id.ToString()
            });
        
        return user.Id;
    }
}
