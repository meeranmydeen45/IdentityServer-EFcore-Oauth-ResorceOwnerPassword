using ClientAppOne.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"c:\PATH TO COMMON KEY APPLE FOLDER"))
    .SetApplicationName("SharedIdentityCookieApp");

builder.Services.AddAuthentication("Bearer")
    //.AddCookie("Identity.Application", o =>
    //{
    //    o.Cookie.Name = "IdentityServer.AppleCookie";
    //})
    .AddJwtBearer("Bearer", o =>
    {
        o.Authority = "https://localhost:4000/";
        o.Audience = "APIone";
    });

//builder.Services.AddAuthentication(o =>
//{
//    o.DefaultScheme = "IdentityServer.AppleCookie";
//    o.DefaultChallengeScheme = "oidc";
//})
//    .AddCookie("IdentityServer.AppleCookie")
//    .AddOpenIdConnect("oidc", o =>
//    {
//        o.Authority = "https://localhost:4000";
//        o.ClientId = "Client1";
//        o.ClientSecret = "Client1Secret";
//        o.ResponseType = "code";

//        o.SaveTokens = true;

//        o.Scope.Add("api1.read");
//        o.Scope.Add("apione.claims");


//        //o.ClaimActions.MapUniqueJsonKey("orange.cookie", "user.role");
//        //o.GetClaimsFromUserInfoEndpoint = true;
//    });


builder.Services.AddCors(p =>
{
    p.AddPolicy("AllowAny", o =>
    {
        o.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

//builder.Services.AddAuthorization(config =>
//{
//    config.AddPolicy("adminPolicy", p =>
//    {
//        p.RequireAuthenticatedUser()
//            .RequireClaim("User.Role", "Admin")
//            .RequireClaim("scope", "API1.read")
//            .RequireClaim("scope", "API1.write");
//    });

//    config.AddPolicy("userPolicy", p =>
//    {
//        p.RequireAssertion(context =>
//        (context.User.HasClaim(x => x.Type == "User.Role" && x.Value == "master")
//        && context.User.HasClaim(x => x.Type == "scope" && x.Value == "API1.read"))
//        || context.User.HasClaim(x => x.Type == "User.Role" && x.Value == "Admin")
//        );
//    });

//    config.AddPolicy("clientPolicy", p =>
//    {
//        p.RequireAssertion(context =>
//        (context.User.HasClaim(x => x.Type == "scope" && x.Value == "API1.write")
//        && context.User.HasClaim(x => x.Type == "scope" && x.Value == "API1.read"))
//        );
//    });
//});

builder.Services.AddHttpClient().AddHttpContextAccessor();

builder.Services.AddControllersWithViews(
    o =>
{
    AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
    o.Filters.Add(new AuthorizeFilter(policy));
    o.Filters.Add(typeof(AuthorizationFilter));

}
);
var app = builder.Build();


app.UseCors("AllowAny");
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(e =>
{
    e.MapDefaultControllerRoute();
});

app.Run();
