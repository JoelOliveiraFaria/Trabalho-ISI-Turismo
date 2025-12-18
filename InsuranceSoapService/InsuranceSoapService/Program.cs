using SoapCore;
using InsuranceSoapService.Interfaces; 
using InsuranceSoapService.Services;  

var builder = WebApplication.CreateBuilder(args);

// --- 1. REGISTAR O SERVIÇO ---
builder.Services.AddSingleton<IInsuranceService, InsuranceService>();

var app = builder.Build();

// --- 2. CRIAR O ENDPOINT SOAP ---
app.UseSoapEndpoint<IInsuranceService>("/Service.asmx", new SoapEncoderOptions());

app.MapGet("/", () => "O Servidor SOAP de Seguros está ativo! Vai a /Service.asmx?wsdl");

app.Run();