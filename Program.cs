using Microsoft.EntityFrameworkCore;
using RentCar.Models;
using RentCar.Utility; // Make sure to use the correct namespace for RentCarContext

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register RentCarContext with dependency injection and configure PostgreSQL.
builder.Services.AddDbContext<RentCarContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // Ensure the connection string is correct

builder.Services.AddScoped<SozlesmeService>();

builder.Services.AddAuthentication("AdminCookie")
    .AddCookie("AdminCookie", options =>
    {
        options.LoginPath = "/Admin/Login"; // Redirect to login page
        options.AccessDeniedPath = "/Admin/AccessDenied"; // Redirect if access is denied
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Login}/{id?}");

app.Run();
