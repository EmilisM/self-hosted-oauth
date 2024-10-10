using Auth.Context;
using Auth.GraphQL.Queries;
using Auth.Models;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder().AddUserSecrets<Program>().AddEnvironmentVariables().Build();

builder.Configuration.AddConfiguration(config);

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
    });

builder.Services.AddControllers();

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "LocalhostOrigin",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("https://localhost:5200").AllowCredentials();
        }
    );
});

builder
    .Services.AddGraphQLServer()
    .AddQueryType()
    .AddMutationType()
    .AddAuthTypes()
    .AddMutationConventions();

var app = builder.Build();

app.UseRouting();
app.UseCors("LocalhostOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "auth-api");

app.MapGraphQL();
app.Run();

app.Run();
