using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OauthServer;
using OauthServer.Services;
using System;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer("server=.;database=OauthDb;Trusted_Connection=true;TrustServerCertificate=True");
});

builder.Services.AddTransient<IResourceOwnerPasswordValidator, UserResourceOwnerPasswordValidator>();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"c:\PATH TO COMMON KEY APPLE FOLDER"))
    .SetApplicationName("SharedIdentityCookieApp");

services.AddIdentity<IdentityUser, IdentityRole>(o =>
{
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequireDigit = false;
    o.Password.RequiredLength = 4;
    o.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

services.AddIdentityServer()
.AddAspNetIdentity<IdentityUser>()
.AddClientStore<ClientStore>()
.AddResourceStore<ResourceStore>()
.AddDeveloperSigningCredential();

services.ConfigureApplicationCookie(o =>
{
    o.Cookie.SameSite = SameSiteMode.None;
    //o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    o.Cookie.Name = "IdentityServer.AppleCookie";
    o.LoginPath = "/Auth/Login";
    o.Cookie.Path = "/";
});


services.AddAuthentication()
    .AddFacebook(o =>
    {
        o.AppId = "310255065080995";
        o.AppSecret = "db1a7ddd3796671eb3936045a099b43a";
    })
    .AddGoogle(o =>
    {
        o.ClientId = "60450826114-0pslri8tso08mduscdduoj0htu7m8arp.apps.googleusercontent.com";
        o.ClientSecret = "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";

        o.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
        o.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
        o.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
        // o.Scope.Add("https://www.googleapis.com/auth/user.birthday.read");
        o.SaveTokens = true;

        o.Events.OnCreatingTicket = ctx =>
        {
            List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();

            tokens.Add(new AuthenticationToken()
            {
                Name = "TicketCreated",
                Value = DateTime.UtcNow.ToString()
            });

            ctx.Properties.StoreTokens(tokens);

            return Task.CompletedTask;
        };
    });

services.AddCors(o =>
{
    o.AddPolicy("AllowAll", o =>
    {
        o.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod();

    });
});

services.AddHttpClient().AddHttpContextAccessor();
services.AddControllersWithViews();

var app = builder.Build();

app.CreateUser();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute(); 
});

app.Run();
