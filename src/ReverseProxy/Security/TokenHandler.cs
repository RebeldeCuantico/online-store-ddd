using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace ReverseProxy.Security
{
    public class TokenHandler
    {
        private readonly ILogger<TokenHandler> _logger;

        public TokenHandler(ILogger<TokenHandler> logger)
        {
            _logger = logger;
        }

        public void Handle(TokenValidatedContext context)
        {
            //TODO validación token

            var accessToken = context.TokenEndpointResponse.AccessToken;
            var idToken = context.TokenEndpointResponse.IdToken;
            var refreshToken = context.TokenEndpointResponse.RefreshToken;
            var expiresIn = context.TokenEndpointResponse.ExpiresIn;
            var expiresAt = new DateTimeOffset(DateTime.Now).AddSeconds(Convert.ToInt32(expiresIn));

            _logger.LogInformation($"Acess Token: {accessToken}");
            _logger.LogInformation($"Id Token: {idToken}");
            _logger.LogInformation($"Refresh Token: {refreshToken}");

            context.HttpContext.Session.SetString(SessionKeys.AccessToken, accessToken);
            context.HttpContext.Session.SetString(SessionKeys.IdToken, idToken);
            context.HttpContext.Session.SetString(SessionKeys.RefreshToken, refreshToken);
            context.HttpContext.Session.SetString(SessionKeys.ExpiresAt, $"{expiresAt.ToUnixTimeSeconds()}");
        }
    }
}
