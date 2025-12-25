/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: SoapInsuranceService.cs
 * Descrição: Implementação do serviço de seguros (IInsuranceService).
 * Responsável por consumir um Web Service SOAP (WCF/Legacy) para calcular custos.
 * ===================================================================================
 */

using TravelAPI.Interfaces;

namespace TravelAPI.Services
{
    /// <summary>
    /// Serviço que comunica com um Web Service SOAP para cálculo de seguros.
    /// Utiliza o cliente gerado automaticamente (Connected Services).
    /// </summary>
    public class SoapInsuranceService : IInsuranceService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor que injeta as configurações da aplicação.
        /// </summary>
        /// <param name="configuration">Acesso ao appsettings.json para ler o URL do serviço SOAP.</param>
        public SoapInsuranceService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Calcula o valor do seguro de viagem com base no orçamento.
        /// Realiza a conversão de tipos (decimal -> double -> decimal) exigida pelo SOAP.
        /// </summary>
        /// <param name="budget">O orçamento da viagem.</param>
        /// <returns>O valor do seguro ou 0 em caso de erro.</returns>
        public async Task<decimal> CalculateInsuranceAsync(decimal budget)
        {
            try
            {
                var soapUrl = _configuration["InsuranceSoupServiceUrl"];

                // Validação de segurança
                if (string.IsNullOrEmpty(soapUrl))
                {
                    Console.WriteLine("URL SOAP não configurado.");
                    return 0;
                }

                // Instanciação do Cliente SOAP gerado pelo Visual Studio
                var client = new SoapInsuranceClient.InsuranceServiceClient(
                    SoapInsuranceClient.InsuranceServiceClient.EndpointConfiguration.BasicHttpBinding_IInsuranceService,
                    soapUrl
                );

                double budgetDouble = (double)budget;

                // Chamada assíncrona ao método remoto
                double result = await client.CalculateInsuranceAsync(budgetDouble);

                // Reconversão para o tipo da nossa aplicação
                return (decimal)result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro SOAP: {ex.Message}");
                return 0;
            }
        }
    }
}