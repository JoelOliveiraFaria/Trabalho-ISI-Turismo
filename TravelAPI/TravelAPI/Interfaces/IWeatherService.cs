namespace TravelAPI.Interfaces
{
    public interface IWeatherService
    {
        Task<string> GetWeatherAsync(string city);
    }
}
