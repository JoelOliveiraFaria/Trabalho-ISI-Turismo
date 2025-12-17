using System;
using TravelAPI.Interfaces;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace TravelAPI.Services
{
    public class OpenWeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        private const string API_KEY = "c088c3afe645b26323e98baacf7a3e1d";

        public OpenWeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetWeatherAsync(string city)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={API_KEY}&units=metric&lang=pt";
            

            try
            {
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                {
                    return null!;
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonObject.Parse(jsonString);

                string description = data!["weather"]?[0]?["description"]?.ToString()!;
                string temp = data["main"]?["temp"]?.ToString()!;

                if(!string.IsNullOrEmpty(description))
                {
                    description = char.ToUpper(description[0]) + description.Substring(1);
                }

                return $"{description}, {temp}ºC";


               
            }catch (Exception ex)
            {
                return ex.Message;
            }
            
        }
    }
}
