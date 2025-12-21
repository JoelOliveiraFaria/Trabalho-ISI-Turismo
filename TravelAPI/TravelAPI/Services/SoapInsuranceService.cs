using TravelAPI.Interfaces;

namespace TravelAPI.Services
{
    public class SoapInsuranceService : IInsuranceService
    {
        private readonly IConfiguration _configuration;

        public SoapInsuranceService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<decimal> CalculateInsuranceAsync(decimal budget)
        {
            try
            {
                var soapUrl = _configuration["InsuranceSoupServiceUrl"];

                if (string.IsNullOrEmpty(soapUrl))
                {
                    Console.WriteLine("ERRO: InsuranceServiceUrl não configurado!");
                    return 0;
                }

                var client = new SoapInsuranceClient.InsuranceServiceClient(
                                    SoapInsuranceClient.InsuranceServiceClient.EndpointConfiguration.BasicHttpBinding_IInsuranceService,
                                    soapUrl
                                );

                double budgetDouble = (double)budget;

                double result = await client.CalculateInsuranceAsync(budgetDouble);

                return (decimal)result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}
