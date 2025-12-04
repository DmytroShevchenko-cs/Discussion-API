namespace Discussion.Web.Controllers.Captcha;

using Base;
using Core.Infrastructure.Constants.Captcha;
using Core.Services.CaptchaService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class CaptchaController(ICaptchaService captchaService) : BaseApiController
{
    [HttpGet]
    [SwaggerOperation("Generate captcha")]
    [Produces("application/json")]
    [Route("api/captcha/generate")]
    [ProducesResponseType(typeof(byte[]), 200)]
    public async Task<IActionResult> Generate()
    {
        var (key, imageBytes) = await captchaService.GenerateCaptchaAsync();
        
        return Ok(new
        {
            key,
            image = $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}"
        });
    }
    
    [HttpPost]
    [SwaggerOperation("Verify captcha")]
    [Produces("application/json")]
    [Route("api/captcha/verify")]
    [ProducesResponseType(typeof(bool), 200)]
    public IActionResult Verify([FromForm] string userInput, [FromForm] string key)
    {
        if (captchaService.VerifyCaptcha(key, userInput))
        {
            return Ok(new { success = true });
        }

        return Ok(new { success = false });
    }
}