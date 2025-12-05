namespace Discussion.Core.Services.CaptchaService;

using Microsoft.Extensions.Caching.Memory;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;

public class CaptchaService(IMemoryCache cache) : ICaptchaService
{
    private readonly Random _random = new();

    public async Task<(string Key, byte[] Image)> GenerateCaptchaAsync(int length = 5)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var code = new string(Enumerable.Range(0, length)
            .Select(_ => chars[_random.Next(chars.Length)])
            .ToArray());

        var key = Guid.NewGuid().ToString();

        var imageBytes = await GenerateImageAsync(code);

        cache.Set(key, code, TimeSpan.FromMinutes(5));

        return (key, imageBytes);
    }

    public bool VerifyCaptcha(string key, string userInput)
    {
        if (!cache.TryGetValue(key, out string? realCode))
        {
            return false;
        }

        return realCode == userInput;
    }

    private Task<byte[]> GenerateImageAsync(string captchaCode)
    {
        return Task.Run(() =>
        {
            var width = 150;
            var height = 50;

            using var image = new Image<Rgba32>(width, height);
            image.Mutate(ctx =>
            {
                ctx.Fill(Color.White);

                var font = SystemFonts.CreateFont("DejaVu Sans", 25, FontStyle.Bold);
                ctx.DrawText(captchaCode, font, Color.Black, new PointF(10, 10));

                for (var i = 0; i < 5; i++)
                {
                    var start = new PointF(_random.Next(width), _random.Next(height));
                    var end = new PointF(_random.Next(width), _random.Next(height));

                    var pathBuilder = new SixLabors.ImageSharp.Drawing.PathBuilder();
                    pathBuilder.AddLine(start, end);
                    var path = pathBuilder.Build();

                    ctx.Draw(Color.Gray, 1f, path);
                }
            });

            using var ms = new MemoryStream();
            image.Save(ms, new PngEncoder());
            return ms.ToArray();
        });
    }
}