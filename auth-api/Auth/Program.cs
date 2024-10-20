using Auth.Context;
using Auth.Models;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using Vite.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder().AddUserSecrets<Program>().AddEnvironmentVariables().Build();
builder.Configuration.AddConfiguration(config);

builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AuthDbContext>();

builder
    .Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder
    .Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore().UseDbContext<AuthDbContext>();
    })
    .AddServer(options =>
    {
        options.SetTokenEndpointUris("connect/token");
        options.SetAuthorizationEndpointUris("connect/authorize");
        options.SetIntrospectionEndpointUris("connect/introspect");
        options.SetUserinfoEndpointUris("connect/userinfo");
        options.AllowAuthorizationCodeFlow();
        options.AllowClientCredentialsFlow();

        options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();

        options.UseReferenceAccessTokens();

        options.RegisterScopes(OpenIddictConstants.Scopes.Email);

        options
            .UseAspNetCore()
            .EnableTokenEndpointPassthrough()
            .EnableAuthorizationEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();

        options.UseAspNetCore();
    });

builder
    .Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.Cookie.Name = "AuthCookie";
        options.LoginPath = "/login";
    });

builder.Services.AddControllers();

builder.Services.AddAuthorization();

builder
    .Services.AddGraphQLServer()
    .AddQueryType()
    .AddMutationType()
    .AddAuthTypes()
    .AddMutationConventions();

builder.Services.AddViteServices(options =>
{
    options.Server.AutoRun = true;
    options.Server.Https = true;
    options.Server.UseReactRefresh = true;
    options.Server.PackageManager = "pnpm";
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapRazorPages();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseWebSockets();
    app.UseViteDevelopmentServer(true);
}

app.Run();
