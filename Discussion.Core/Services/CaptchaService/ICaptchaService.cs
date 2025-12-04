namespace Discussion.Core.Services.CaptchaService;

public interface ICaptchaService
{
    Task<string> GenerateCaptchaCodeAsync(int length = 5);
    Task<byte[]> GenerateCaptchaImageAsync(string captchaCode);
}