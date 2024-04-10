using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("Cookie")
    .AddCookie("Cookie", options => options.LoginPath = "/account/login");
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", builder =>
    {
        builder.RequireClaim(ClaimTypes.Role, "admin");
    });
    options.AddPolicy("User", builder =>
    {
        builder.RequireClaim(ClaimTypes.Role, "user");
    });
    options.AddPolicy("Writer", builder =>
    {
        builder.RequireAssertion(context => (context.User.HasClaim(ClaimTypes.Role, "admin") ||
                                            context.User.HasClaim(ClaimTypes.Role, "writer")));
    });
});


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
