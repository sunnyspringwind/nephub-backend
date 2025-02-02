using Microsoft.EntityFrameworkCore;
using NepHubAPI.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register Services
builder.Services.AddDbContext<NepHubContext>(options => 
    options.UseNpgsql(connString));
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NepHubAPI", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NepHubAPI v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

// Map controllers
app.MapControllers();

app.Run();
