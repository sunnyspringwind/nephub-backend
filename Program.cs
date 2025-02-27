using Microsoft.EntityFrameworkCore;
using NepHubAPI.Data;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NepHubAPI.Models;
using Microsoft.AspNetCore.Identity;
using NepHubAPI.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NepHubAPI", Version = "v1" });
});

var connString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register Services
builder.Services.AddDbContext<NepHubContext>(options => 
    options.UseNpgsql(connString));
builder.Services.AddControllers().AddNewtonsoftJson();


// Register Identity User
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<NepHubContext>()
.AddDefaultTokenProviders();

// Register token service
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var config = builder.Configuration;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["JWT:Issuer"],
        ValidAudience = config["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SigningKey"]!)),
        ClockSkew = TimeSpan.Zero // ⏰ No grace period
    };
});

builder.Services.AddControllers();
builder.Services.AddAuthorization();



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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
