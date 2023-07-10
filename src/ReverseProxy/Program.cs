using Common.Infrastructure.ServiceDiscovery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ReverseProxy.Security;
using ReverseProxy.Settings;
using Steeltoe.Extensions.Configuration.ConfigServer;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfigServer();
builder.Services.Configure<ServiceDiscoverySettings>(builder.Configuration.GetSection(nameof(ServiceDiscoverySettings)));
builder.Services.Configure<OpenIdConnectSettings>(builder.Configuration.GetSection(nameof(OpenIdConnectSettings)));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddSingleton<TokenHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("auth", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})

.AddCookie(setup =>
{
    setup.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    setup.SlidingExpiration = true;
    setup.LoginPath = "/login";
    setup.AccessDeniedPath = "/login";
    setup.ReturnUrlParameter = "redirectUrl";
})
.AddOpenIdConnect(options =>
{
    var openIdConnectSettingsOptions = builder.Services.BuildServiceProvider().GetService<IOptions<OpenIdConnectSettings>>();
    var openIdConnectSettings = openIdConnectSettingsOptions.Value;
    var httpClient = new HttpClient();
    var docJson = httpClient.GetStringAsync(openIdConnectSettings.DiscoveryUrl).Result;
    var doc = new IdpServiceDiscoveryConfiguration
    {
        TokenEndpoint = JsonDocument.Parse(docJson).RootElement.GetProperty("token_endpoint").ToString(),
    };

    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.Authority = openIdConnectSettings.Authority;
    options.ClientId = openIdConnectSettings.ClientId;
    options.ClientSecret = openIdConnectSettings.ClientSecret;
    options.UsePkce = true;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.SaveTokens = false;
    options.GetClaimsFromUserInfoEndpoint = false;
    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
    options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
    options.RequireHttpsMetadata = false;

    var scopeArray = openIdConnectSettings.Scopes.Split(" ");
    foreach (var scope in scopeArray)
    {
        options.Scope.Add(scope);
    }

    options.Events.OnTokenValidated = (ctx) =>
    {
        var tokenHalder = ctx.HttpContext.RequestServices.GetRequiredService<TokenHandler>();
        tokenHalder.Handle(ctx);
        return Task.FromResult(0);
    };
});

var serviceDiscoveryOptions = builder.Services.BuildServiceProvider().GetService<IOptions<ServiceDiscoverySettings>>();
builder.Services.AddServiceDiscovery(serviceDiscoveryOptions);
builder.Services.AddInternalReverseProxy(serviceDiscoveryOptions, builder.Services.BuildServiceProvider().GetRequiredService<IServiceDiscovery>());



var app = builder.Build();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();

app.MapGet("/login", (string? redirectUrl, HttpContext ctx) => 
{
    if(string.IsNullOrEmpty(redirectUrl))
    {
        redirectUrl = "/";
    }

    ctx.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = redirectUrl
    });
});

app.MapGet("/userinfo", (ClaimsPrincipal user) =>
{
    var claims = user.Claims;
    var dictionary = new Dictionary<string, string>();

    foreach (var claim in claims)
    {
        dictionary[claim.Type] = claim.Value;
    }

    return dictionary;
});

app.MapReverseProxy();

app.Run();




