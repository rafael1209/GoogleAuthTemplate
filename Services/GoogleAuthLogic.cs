using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth;
using WorkingHoursCounterSystemCore.DataWrapper;
using WorkingHoursCounterSystemCore.Models;
using WorkingHoursCounterSystemCore.Interfaces;

namespace WorkingHoursCounterSystemCore.Services
{
    public class GoogleAuthLogic
    {
        private AuthorizeDbContext _authorizeDbContext;
        private ITokenService _tokenService;

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;

        public GoogleAuthLogic(AuthorizeDbContext authorizeDbContext, ITokenService tokenService, IConfiguration configuration)
        {
            _clientId = configuration.GetValue<string>("GoogleAuthCredentials:ClientId")!;

            _clientSecret = configuration.GetValue<string>("GoogleAuthCredentials:ClientSecret")!;

            _redirectUri = configuration.GetValue<string>("GoogleAuthCredentials:RedirectUri")!;

            _authorizeDbContext = authorizeDbContext;

            _tokenService = tokenService;
        }

        public User GetUser(string AuthToken)
        {
            return _authorizeDbContext.GetUserByToken(AuthToken);
        }

        public Uri GetGoogleAuthUrl()
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

            return authorizationUrl;
        }

        public async Task<(bool Success, object Result)> HandleGoogleAuthCallbackAsync(string code)
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
                    return (false, new { error = "ID token is missing" });
                }

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings());

                string authToken = _tokenService.GetToken(payload.Subject);

                if (_authorizeDbContext.IsUserExistByEmail(payload.Email))
                {
                    var user = _authorizeDbContext.GetUserByEmail(payload.Email);

                    authToken = user.AuthToken;
                }
                else
                {
                    _authorizeDbContext.AddUser(new Models.User { Email = payload.Email, Name = payload.Name, AvatarUrl = payload.Picture }, authToken);
                }

                return (true, authToken);
            }
            catch (Exception ex)
            {
                return (false, new { error = ex.Message });
            }
        }
    }
}
