using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TravelAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVIÇOS ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<TravelAPI.Interfaces.IWeatherService, TravelAPI.Services.OpenWeatherService>();

// --- NOVA CONFIGURAÇÃO (NSwag) ---
// Substitui o AddSwaggerGen e evita o erro de versão
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "v1";
    config.Title = "TravelAPI";
    config.Version = "v1";
    config.Description = "API para Travel Planner";

    // Configuração para suportar o token JWT no botão "Authorize" do Swagger
    config.AddSecurity("Bearer", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Description = "Coloque: Bearer {seu_token_aqui}"
    });
});
// ----------------------------------

// Configuração da Base de Dados
builder.Services.AddDbContext<TravelPlannerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TravelDatabase"))
);

// Configuração da Autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// --- 2. PIPELINE ---

if (app.Environment.IsDevelopment())
{
    // --- NOVA CONFIGURAÇÃO (NSwag) ---
    app.UseOpenApi();       // Gera o ficheiro swagger.json
    app.UseSwaggerUi();     // Gera a interface gráfica (acessa em /swagger)
    // ----------------------------------
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();