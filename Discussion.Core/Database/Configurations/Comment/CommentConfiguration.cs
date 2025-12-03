namespace Discussion.Core.Database.Configurations.Comment;

using Discussion.Core.Database.Entities.Comment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasMany(r => r.Files)
            .WithOne(r => r.Comment)
            .HasForeignKey(r => r.CommentId);
        
        builder.HasMany(r => r.Images)
            .WithOne(r => r.Comment)
            .HasForeignKey(r => r.CommentId);
        
        builder.Property(r => r.Text)
            .HasMaxLength(2000);
        
        builder.Property(r => r.IsDeleted)
            .HasDefaultValue(false);
    }
}