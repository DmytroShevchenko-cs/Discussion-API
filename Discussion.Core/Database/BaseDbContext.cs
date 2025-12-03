namespace Discussion.Core.Database;

using Entities.Comment;
using Entities.Storage;
using Entities.User;
using Extensions;
using Microsoft.EntityFrameworkCore;

public class BaseDbContext(DbContextOptions<BaseDbContext> options) : DbContext(options)
{
    public DbSet<Comment> Comments { get; set; }
    public DbSet<StorageItem> StorageItems { get; set; }
    public DbSet<CommentFile> CommentFiles { get; set; }
    public DbSet<CommentImage> CommentImages { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseDbContext).Assembly);
        modelBuilder.AddPostgreSqlRules();
    }
}