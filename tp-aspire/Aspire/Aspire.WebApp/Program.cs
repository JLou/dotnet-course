using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Aspire.ServiceDefaults;
using Aspire.WebApp;
using Aspire.WebApp.Clients;
using Aspire.WebApp.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using TokenHandler = Aspire.WebApp.TokenHandler;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenHandler>();

builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

builder
    .Services.AddHttpClient<ITodoClient, TodoClient>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new("https+http://apiservice");
    })
    .AddHttpMessageHandler<TokenHandler>();

// Ajout de l'authentification OIDC
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(
        "oidc",
        options =>
        {
            options.Authority = builder.Configuration["Authentication:OIDC:Authority"];
            options.ClientId = builder.Configuration["Authentication:OIDC:ClientId"];
            options.RequireHttpsMetadata = false;
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.CallbackPath = "/signin-oidc";
            options.SignedOutCallbackPath = "/signout-callback-oidc";
            options.UseTokenLifetime = true;
            options.MapInboundClaims = false;
            options.Scope.Add("api");
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                NameClaimType = "name",
                RoleClaimType = "role",
            };

            options.ClaimActions.MapAll();
        }
    );

builder.Services.ConfigureCookieOidc(CookieAuthenticationDefaults.AuthenticationScheme, "oidc");
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapGroup("/authentication").MapLoginAndLogout();

app.Run();
