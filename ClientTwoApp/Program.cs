using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"c:\PATH TO COMMON KEY APPLE FOLDER"))
    .SetApplicationName("SharedIdentityCookieApp");

builder.Services.AddAuthentication("Identity.Application")
    .AddCookie("Identity.Application", o =>
    {
        o.Cookie.Name = "IdentityServer.AppleCookie";
    });

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


var app = builder.Build();

app.UseRouting();
app.UseCors("AllowAny");
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute(); 

});

app.Run();
