using Inl�mning_API.Infrastructure.Profiles;
using Inl�mning_API.Model;
using Inl�mning_API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var _Config = builder.Configuration;
var _Services = builder.Services;

var jwtSettings = _Config.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
var connectionString = _Config.GetConnectionString("DefaultConnection");

_Services.AddControllers().AddNewtonsoftJson();
_Services.AddEndpointsApiExplorer();
_Services.AddDbContext<APIDbContext>(options =>
    options.UseSqlServer(connectionString)
);
_Services.AddTransient<DataInitialize>();
_Services.AddAutoMapper(typeof(AdsProfile));
_Services.AddAutoMapper(typeof(JwtSettings));
_Services.AddSwaggerGen();

//==================================================
_Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = jwtSettings.ValidateIssuer,
            ValidateAudience = jwtSettings.ValidateAudience,
            ValidateLifetime = jwtSettings.ValidateLifetime,
            ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
            ValidIssuer = jwtSettings.ValidIssuer,
            ValidAudience = jwtSettings.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });
//==================================================

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<DataInitialize>().SeedData();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//==================================================
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
//==================================================

app.MapControllers();

app.Run();
