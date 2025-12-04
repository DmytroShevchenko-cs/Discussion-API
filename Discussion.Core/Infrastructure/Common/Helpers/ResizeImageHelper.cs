namespace Discussion.Core.Infrastructure.Common.Helpers;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

public static class ResizeImageHelper
{
    private const int MaxWidth = 320;
    private const int MaxHeight = 240;
    
    public static async Task<byte[]> ResizeImageAsync(Stream input, int maxWidth = MaxWidth, int maxHeight = MaxHeight)
    {
        using var msInput = new MemoryStream();
        await input.CopyToAsync(msInput);
        msInput.Position = 0;
        
        var format = await Image.DetectFormatAsync(msInput) ?? 
                     throw new InvalidOperationException("Unable to determine image format");

        msInput.Position = 0;
        
        using var image = await Image.LoadAsync(msInput);

        if (image.Width > maxWidth || image.Height > maxHeight)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(maxWidth, maxHeight)
            }));
        }

        using var msOutput = new MemoryStream();
        await image.SaveAsync(msOutput, format);
        return msOutput.ToArray();
    }
}