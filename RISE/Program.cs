using Microsoft.EntityFrameworkCore;
using RISE.Data;
using RISE.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add EF Core InMemory DB
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("RiseDb"));

// Add cookie authentication for admin
builder.Services.AddAuthentication("AdminCookie")
                .AddCookie("AdminCookie", options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/Login";
                });

var app = builder.Build();

// Seed default admin user
using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if(!db.Users.Any())
    {
        db.Users.Add(new User
        {
            Username = "admin",
            Password = "password"  // Solo per test, NON in produzione
        });
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();