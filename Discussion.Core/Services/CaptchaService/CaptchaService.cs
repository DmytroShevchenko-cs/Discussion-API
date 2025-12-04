namespace Discussion.Core.Services.CaptchaService;

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;

public class CaptchaService : ICaptchaService
{
    private readonly Random _random = new ();

    public Task<string> GenerateCaptchaCodeAsync(int length = 5)
    {
        return Task.FromResult(new string(Enumerable.Range(0, length)
            .Select(_ => "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"[_random.Next(36)]).ToArray()));
    }

    public Task<byte[]> GenerateCaptchaImageAsync(string captchaCode)
    {
        return Task.Run(() =>
        {
            int width = 150, height = 50;

            using var image = new Image<Rgba32>(width, height);
            image.Mutate(ctx =>
            {
                ctx.Fill(Color.White);

                var font = SystemFonts.CreateFont("Arial", 25, FontStyle.Bold);
                ctx.DrawText(captchaCode ?? "", font, Color.Black, new PointF(10, 10));
                
                for (int i = 0; i < 5; i++)
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