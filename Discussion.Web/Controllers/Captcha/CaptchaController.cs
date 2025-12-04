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
        var code = await captchaService.GenerateCaptchaCodeAsync();
        HttpContext.Session.SetString(CaptchaConsts.CaptchaCode, code);

        var imageBytes = await captchaService.GenerateCaptchaImageAsync(code);
        return File(imageBytes, "image/png");
    }
    
    [HttpPost]
    [SwaggerOperation("Verify captcha")]
    [Produces("application/json")]
    [Route("api/captcha/verify")]
    [ProducesResponseType(typeof(bool), 200)]
    public IActionResult Verify([FromForm] string userInput)
    {
        var savedCode = HttpContext.Session.GetString(CaptchaConsts.CaptchaCode);
        
        if (string.IsNullOrEmpty(userInput) || 
            savedCode == null || 
            userInput.ToUpper() != savedCode.ToUpper())
        {
            return Ok(false);
        }

        return Ok(true);
    }
}