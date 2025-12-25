using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TravelAPI.Data;
using TravelAPI.Interfaces;
using TravelAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVIÇOS ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<IWeatherService, OpenWeatherService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5002/");
});
builder.Services.AddScoped<TravelAPI.Interfaces.IInsuranceService, TravelAPI.Services.SoapInsuranceService>();

builder.Services.AddHttpClient<TravelAPI.Interfaces.ICalendarService, TravelAPI.Services.RemoteCalendarService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7083/");
});

// 4. Serviço de Email (Remoto)
builder.Services.AddHttpClient<TravelAPI.Interfaces.IEmailService, TravelAPI.Services.RemoteEmailService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7205/");
});

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});



var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TravelAPI.Data.TravelPlannerContext>();
        context.Database.EnsureCreated();

        if (!context.Destinations.Any())
        {
            context.Destinations.AddRange(
                new TravelAPI.Models.Destination { City = "Paris", Country = "France", Description = "A Cidade da Luz e do amor." },
                new TravelAPI.Models.Destination { City = "London", Country = "United Kingdom", Description = "História, cultura e o Big Ben." },
                new TravelAPI.Models.Destination { City = "New York", Country = "USA", Description = "A cidade que nunca dorme." },
                new TravelAPI.Models.Destination { City = "Lisbon", Country = "Portugal", Description = "Sol, fado e pastéis de Belém." },
                new TravelAPI.Models.Destination { City = "Tokyo", Country = "Japan", Description = "Onde a tradição encontra o futuro." },
                new TravelAPI.Models.Destination { City = "Rome", Country = "Italy", Description = "O Coliseu, pizza e muita história." },
                new TravelAPI.Models.Destination { City = "Sydney", Country = "Australia", Description = "Praias, surf e a Opera House." },
                new TravelAPI.Models.Destination { City = "Rio de Janeiro", Country = "Brazil", Description = "Samba, Carnaval e o Cristo Redentor." },
                new TravelAPI.Models.Destination { City = "Berlin", Country = "Germany", Description = "Arte urbana, história e vida noturna." },
                new TravelAPI.Models.Destination { City = "Cape Town", Country = "South Africa", Description = "Montanhas, pinguins e natureza selvagem." }
                );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// --- 2. PIPELINE ---

//if (app.Environment.IsDevelopment())
//{
    // --- NOVA CONFIGURAÇÃO (NSwag) ---
    app.UseOpenApi();       // Gera o ficheiro swagger.json
    app.UseSwaggerUi();     // Gera a interface gráfica (acessa em /swagger)
    // ----------------------------------
//}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();