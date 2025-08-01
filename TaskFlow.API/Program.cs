// TaskFlow.API/Program.cs

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskFlow.API;
using TaskFlow.API.Data;

var builder = WebApplication.CreateBuilder(args);

// --- DEFINE CORS POLICY ---
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// --- ADD CORS SERVICES ---
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // IMPORTANT: Use the correct URL for your WebApp
                          policy.WithOrigins("https://localhost:7049")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});


// --- Add services to the container. ---
var jwtSettings = new JwtSettings();
builder.Configuration.Bind("Jwt", jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { /* ... */ });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement { /* ... */ });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- USE THE CORS POLICY ---
app.UseCors(MyAllowSpecificOrigins); // This must go before UseAuthentication/UseAuthorization

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
