using Microsoft.AspNetCore.Authentication.Cookies;
using System.Configuration;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<INewsService, NewsService>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<ITeamService, TeamService>();
builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<IEventService, EventService>();
builder.Services.AddTransient<IMatchService, MatchService>();
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

//app.MapGet("/", (ApplicationDbContext db) => db.Users.ToList());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
