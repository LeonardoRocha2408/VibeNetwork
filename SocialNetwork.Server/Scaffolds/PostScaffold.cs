using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Server.Domain.Entities;

public class PostScaffold
{
    private const byte ContentMinLength = 1;
    
    [Column("id")]
    public Guid Id { get; init; } = Guid.CreateVersion7();

    [Column("user_owner:id")]
    public Guid AuthorId { get; init; }
    public UserScaffold Author { get; set; } = null!;

    [Column("content")]
    [MinLength(ContentMinLength)]
    public string Content { get; set; } = string.Empty;
    [Column("likes_count")]
    public int LikesCount { get; set; }
    [Column("coments_count")]
    public int CommentsCount { get; set; }
    [Column("media")]
    public string MediaUrl { get; set; } = string.Empty;
    [Column("visibility")]
    public PostVisibility Visibility { get; private set; }
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
    [Column("deleted_at")]
    public DateTimeOffset DeletedAt { get; set; }

    public PostScaffold() {}
    public PostScaffold(Guid authorId, string content, PostVisibility visibility)
    {
        AuthorId = authorId;
        Content = content;
        Visibility = visibility;
    }

    public (string content, DateTimeOffset updatedAt) UpdatePostContent(string newContent)
    {
        return (Content = newContent, UpdatedAt = DateTimeOffset.UtcNow);
    }
    public void DeletePost() => DeletedAt = DateTimeOffset.UtcNow;
}

public static class PostFluentApi
{
    public static ModelBuilder PostBuilder(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostScaffold>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("post_id");

            entity
            .HasOne(e => e.Author)
            .WithMany()
            .HasForeignKey(e => e.AuthorId)
            .IsRequired();

            entity.Property(e => e.AuthorId)
            .HasColumnName("author_id")
            .IsRequired();

            entity
            .Property(e => e.Content)
            .HasMaxLength(255)
            .HasColumnName("content")
            .IsRequired();
        });

        return modelBuilder;
    }
}
