using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<INewsService, NewsService>();
builder.Services.AddTransient<ICommentService, CommentService>();
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
