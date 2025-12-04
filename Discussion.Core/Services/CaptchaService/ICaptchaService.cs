namespace Discussion.Core.Services.CaptchaService;

public interface ICaptchaService
{
    Task<(string Key, byte[] Image)> GenerateCaptchaAsync(int length = 5);

    bool VerifyCaptcha(string key, string userInput);
}