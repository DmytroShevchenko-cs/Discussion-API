namespace Discussion.Core.Infrastructure.Messages;

public class FileMessageItemModel
{
    public string FileName { get; set; } = null!;
    public byte[] Content { get; set; } = null!;
}