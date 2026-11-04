using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Server.Domain.Entities;

[Table("refresh_tokens")]
public class RefreshTokenScaffold
{
    [Column("id")]
    public Guid Id { get; init; } = Guid.NewGuid();
    [Column("user_id")]
    public Guid UserId { get; init; }
    public UserScaffold User { get; init; } = null!;
    [Column("token_hash")]
    public string TokenHash { get; init; } = string.Empty;
    [Column("revoked_at")]
    public DateTime? RevokedAt { get; set; }
}

public static class RefreshTokenFluentApi
{
    public static ModelBuilder RefreshTokenBuilder(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshTokenScaffold>(entity =>
        {
            entity.HasKey(rt => rt.Id).HasName("id");

            entity.Property(rt => rt.UserId)
            .HasColumnName("user_id")
            .IsRequired();
            
            entity
            .HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(rt => rt.TokenHash);

            entity.Property(rt => rt.RevokedAt)
            .HasColumnName("revoked_at");
        });

        return modelBuilder;
    }
}