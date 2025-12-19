using Microsoft.EntityFrameworkCore;
using RISE.Data;
using RISE.Models;
using RISE.Security;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// DB InMemory (MVP)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("RiseDb"));

// AUTHENTICATION
builder.Services.AddAuthentication()
    .AddCookie("AdminCookie", options =>
    {
        options.LoginPath = "/Account/AdminLogin";
        options.AccessDeniedPath = "/Account/AdminLogin";
        options.Cookie.Name = "RiseAdminAuth";
    })
    .AddCookie("UserCookie", options =>
    {
        options.LoginPath = "/Account/UserLogin";
        options.AccessDeniedPath = "/Account/UserLogin";
        options.Cookie.Name = "RiseUserAuth";
    });

var app = builder.Build();

// SEED
using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if(!db.Users.Any())
    {
        db.Users.Add(new User
        {
            Username = "admin",
            PasswordHash = PasswordHasher.Hash("password"),
            Role = "Admin"
        });
        db.SaveChanges();
    }

    DbInitializer.Seed(db);
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// AREA ADMIN
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Dashboard}/{id?}");

// PUBLIC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
