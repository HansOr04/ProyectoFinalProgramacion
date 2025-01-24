using APIPugaOrtizLopez.Controllers;
using APIPugaOrtizLopez.Data;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<BddproyectoFinalContext>(builder.Configuration.GetConnectionString("DataConnection"));

builder.Services.AddDistributedMemoryCache(); // Add this line

builder.Services.AddSession(options => {
    options.Cookie.Name = ".APIPugaOrtizLopez.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddDataProtection()
  .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "DataProtection-Keys")))
  .SetApplicationName("APIPugaOrtizLopez");

builder.Services.AddControllers()
  .AddJsonOptions(options => {
      options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
  });

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());  // Remove .AllowCredentials()
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseSession();
app.UseAuthorization();

app.MapControllers();
app.MapUsuarioEndpoints();
app.MapDepartamentoEndpoints();
app.MapComentarioEndpoints();

app.Run();