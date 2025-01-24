using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Configuración de autenticación y sesión
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
   .AddCookie(options =>
   {
       options.LoginPath = "/Usuario/Login";
       options.LogoutPath = "/Usuario/Logout";
   });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configuración de Entity Framework
builder.Services.AddDbContext<BDDProyectoFinal>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("BDDProyectoFinal")
       ?? throw new InvalidOperationException("Connection string 'BDDProyectoFinal' not found.")));

// Configuración de Cloudinary
var cloudinaryConfig = builder.Configuration.GetSection("Cloudinary");
if (cloudinaryConfig["CloudName"] != null && cloudinaryConfig["ApiKey"] != null && cloudinaryConfig["ApiSecret"] != null)
{
    Account cloudinaryAccount = new Account(
        cloudinaryConfig["CloudName"],
        cloudinaryConfig["ApiKey"],
        cloudinaryConfig["ApiSecret"]
    );
    Cloudinary cloudinary = new Cloudinary(cloudinaryAccount);
    builder.Services.AddSingleton(cloudinary);
}
else
{
    throw new InvalidOperationException("Cloudinary configuration not found in appsettings.json.");
}

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Agregar UseAuthentication antes de UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

// Agregar UseSession después de UseRouting
app.UseSession();

app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();