using ConferenciaPotyAPI.Services;
using ConteiAPI.Helpers;
using ConteiAPI.Interfaces;
using ConteiAPI.Interfaces.Repositories;
using ConteiAPI.Interfaces.Services;
using ConteiAPI.Models;
using ConteiAPI.Repositories;
using ConteiAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Vinculando o JWT do appsettings com a classe JwtSettings
builder.Services.Configure<JwtSettings>(
        builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    if (jwtSettings == null)
    {
        throw new ArgumentNullException("JwtSettings is not configured in appsettings.json");
    }
    var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)

    };
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<SAPSettingsModel>(builder.Configuration.GetSection("SAP"));
builder.Services.AddSingleton(provider =>
{
    var settings = provider.GetRequiredService<IOptions<SAPSettingsModel>>().Value;
    return SapConexao.GetInstance(settings);
});



// Repositories
builder.Services.AddScoped<IAutenticacaoRepository, AutenticacaoRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IContagemRepository, ContagemRepository>();


// Services
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddScoped<IEtiquetaService, EtiquetaService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IContagemService, ContagemService>();



// SAP
builder.Services.AddScoped<SapService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
