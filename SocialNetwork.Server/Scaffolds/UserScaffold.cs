using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Server.Domain.Entities;

[Table("users")]
public class UserScaffold
{
    [Column("id")]
    public Guid Id { get; private set; } = Guid.NewGuid();
    [Column("email")]
    public string Email { get; private set; } = string.Empty;
    [Column("display_name")]
    public string DisplayName { get; private set; } = string.Empty;
    [Column("username")]
    public string UserName { get; private set; } = string.Empty;
    [Column("password_hash")]
    public string PasswordHash { get; private set; } = string.Empty;
    [Column("profile_picture_url")]
    public string ProfilePictureURL {  get; private set; } = string.Empty;
    [Column("role")]
    public UserRole Role { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private UserScaffold() { }

    public UserScaffold(string email, string username, string passwordHash)
    {
        Email = email;
        UserName = username;
        DisplayName = username;
        PasswordHash = passwordHash;
    }

    public void UpdateProfilePicture(string profilePictureURL) => ProfilePictureURL = profilePictureURL;
    public void UpdateDisplayName(string displayName) => DisplayName = displayName;
    public void UpdateUserName(string username) => UserName = username;
}

public static class UserFluentApi
{
    public static ModelBuilder UserBuilder(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserScaffold>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity
            .Property(x => x.Email)
            .HasMaxLength(255)
            .HasColumnName("email")
           .IsRequired();
            entity
            .HasIndex(u => u.Email)
            .IsUnique();

            entity
            .Property(u => u.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(70)
            .IsRequired();

            entity
            .Property(u => u.UserName)
            .HasColumnName("username")
            .IsRequired()
            .HasMaxLength(50);

            entity
            .HasIndex(u => u.UserName)
            .IsUnique();

            entity
            .Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired();

            entity
            .Property(u => u.ProfilePictureURL)
            .HasMaxLength(255)
            .HasColumnName("profile_picture_url");

            entity
            .Property(u => u.Role)
            .HasConversion<string>()
            .HasDefaultValue(UserRole.User)
            .HasColumnName("role")
            .IsRequired();
            
            entity
            .Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        });
        return modelBuilder;
    }
}
