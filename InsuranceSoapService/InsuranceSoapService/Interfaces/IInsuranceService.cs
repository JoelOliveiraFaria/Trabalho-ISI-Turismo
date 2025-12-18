using System.ServiceModel;

namespace InsuranceSoapService.Interfaces
{
    [ServiceContract]
    public interface IInsuranceService
    {
        [OperationContract]
        double CalculateInsurance(double budget);
    }
}
