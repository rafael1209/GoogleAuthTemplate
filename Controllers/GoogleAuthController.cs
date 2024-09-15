using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Google.Apis.Auth;

namespace WorkingHoursCounterSystemCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleAuthController : Controller
    {
        private static string _clientId = "CLIENT_ID";
        private static string _clientSecret = "CLIENT_SECRET";
        private static string _redirectUri = "https://localhost:7262/api/GoogleAuth/callback";

        [HttpGet("auth-url")]
        public IActionResult GetGoogleAuthUrl()
        {
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = _clientId,
                    ClientSecret = _clientSecret
                },
                Scopes = new[] { "email", "profile" },
            });

            var authorizationUrl = flow.CreateAuthorizationCodeRequest(_redirectUri)
                                       .Build();

            return Ok(new { authUrl = authorizationUrl });
        }

        [HttpGet("callback")]
        public async Task<IActionResult> GoogleCallback(string code)
        {
            try
            {
                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = _clientId,
                        ClientSecret = _clientSecret
                    }
                });

                var tokenResponse = await flow.ExchangeCodeForTokenAsync("me", code, _redirectUri, CancellationToken.None);
                var idToken = tokenResponse.IdToken;

                if (idToken == null)
                {
                    return BadRequest(new { error = "ID token is missing" });
                }

                var payload = GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings()).Result;

                var userInfo = new
                {
                    UserId = payload.Subject,
                    Email = payload.Email,
                    Name = payload.Name,
                    PictureUrl = payload.Picture
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}