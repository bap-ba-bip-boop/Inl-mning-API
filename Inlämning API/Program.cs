using AutoMapper;
using Inlämning_API.Infrastructure.Profiles;
using Inlämning_API.Model;
using Inlämning_API.Services;
using Inlämning_API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
_Services.AddTransient<IAdAPIService, AdAPIService>();
_Services.AddAutoMapper(typeof(AdsProfile));
_Services.AddSwaggerGen();

{
    var _mapper = new Mapper(
        new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new JwtSettingsProfile());
            }
        )
    );

    _Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = _mapper.Map<TokenValidationParameters>(jwtSettings);
        }
    );
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<DataInitialize>()!.SeedData();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
