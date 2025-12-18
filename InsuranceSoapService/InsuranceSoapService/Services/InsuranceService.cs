using InsuranceSoapService.Interfaces;

namespace InsuranceSoapService.Services
{
    public class InsuranceService :IInsuranceService
    {
        public double CalculateInsurance(double budget)
        {
            // Simple insurance calculation logic
            if (budget < 1000)
            {
                return budget * 0.05; // 5% for budgets under 1000
            }
            else if (budget < 5000)
            {
                return budget * 0.03; // 3% for budgets between 1000 and 5000
            }
            else
            {
                return budget * 0.02; // 2% for budgets over 5000
            }
        }
    }
}
