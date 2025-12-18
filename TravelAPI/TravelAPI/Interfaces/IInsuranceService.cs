namespace TravelAPI.Interfaces
{
    public interface IInsuranceService
    {
        Task<decimal> CalculateInsuranceAsync(decimal budget);
    }
}
