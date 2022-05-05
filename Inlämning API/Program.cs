using AutoMapper;
using Inlämning_API.Infrastructure.Profiles;
using Inlämning_API.Model;
using Inlämning_API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<APIDbContext>(options =>
    options.UseSqlServer(connectionString)
);
builder.Services.AddTransient<DataInitialize>();
builder.Services.AddAutoMapper(typeof(AdsProfile));
builder.Services.AddAutoMapper(typeof(JwtSettings));
builder.Services.AddSwaggerGen();

//==================================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = //_mapper.Map<TokenValidationParameters>(jwtSettings);
        new TokenValidationParameters // kanske kan använda automapper?
        {
            ValidateIssuer = jwtSettings.ValidateIssuer,
            ValidateAudience = jwtSettings.ValidateAudience,
            ValidateLifetime = jwtSettings.ValidateLifetime,
            ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
            ValidIssuer = jwtSettings.ValidIssuer,
            ValidAudience = jwtSettings.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes( jwtSettings.Key ))
        };
    });
//==================================================

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<DataInitialize>().SeedData();
}

// Configure the HTTP request pipeline.
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
