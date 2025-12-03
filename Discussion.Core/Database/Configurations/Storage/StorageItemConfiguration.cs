namespace Discussion.Core.Database.Configurations.Storage;

using Discussion.Core.Database.Entities.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StorageItemConfiguration : IEntityTypeConfiguration<StorageItem>
{
    public void Configure(EntityTypeBuilder<StorageItem> builder)
    {
        builder.HasIndex(x => x.StoragePath).IsUnique();
    }
}