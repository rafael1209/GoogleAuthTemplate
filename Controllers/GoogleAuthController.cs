using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;
using WorkingHoursCounterSystemCore.Services;

namespace WorkingHoursCounterSystemCore.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GoogleAuthController : Controller
    {
        private readonly GoogleAuthLogic _googleAuthLogic;

        public GoogleAuthController(GoogleAuthLogic googleAuthLogic)
        {
            _googleAuthLogic = googleAuthLogic;
        }

        [HttpGet("auth-url")]
        public IActionResult GetGoogleAuthUrl()
        {
            return Ok(_googleAuthLogic.GetGoogleAuthUrl());
        }

        [HttpGet("callback")]
        public async Task<IActionResult> GoogleCallback(string code)
        {
            var (success, result) = await _googleAuthLogic.HandleGoogleAuthCallbackAsync(code);

            return success ? Ok(result) : BadRequest();
        }
    }
}