using TravelAPI.Interfaces;

namespace TravelAPI.Services
{
    public class SoapInsuranceService : IInsuranceService
    {
        public async Task<decimal> CalculateInsuranceAsync(decimal budget)
        {
            try
            {
                var client = new SoapInsuranceClient.InsuranceServiceClient(
                    SoapInsuranceClient.InsuranceServiceClient.EndpointConfiguration.BasicHttpBinding_IInsuranceService,
                    "https://localhost:7252/Service.asmx"
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
