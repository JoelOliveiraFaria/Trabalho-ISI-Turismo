using Newtonsoft.Json.Linq; // Certifique-se que tem o pacote Newtonsoft.Json instalado
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TravelSoapService
{
    public class Service1 : IService1
    {
        public string ImportHotelReservations(DateTime start, DateTime end)
        {
            return ProcessIntegration(start, end).Result;
        }

        private async Task<string> ProcessIntegration(DateTime start, DateTime end)
        {
            using (var client = new HttpClient())
            {
                string sDate = start.ToString("yyyy-MM-dd");
                string eDate = end.ToString("yyyy-MM-dd");

                string apiKey = "693af0b8c60eea10d6a48c21";

                // Construção do URL manual (o que o Axios faz sozinho)
                // CityId 60763 = New York
                string urlHotel = $"https://api.makcorps.com/city?cityid=60763&pagination=0&cur=USD&rooms=1&adults=2&checkin={sDate}&checkout={eDate}&api_key={apiKey}";

                // URL da TUA API REST (Backend) - Confirme a porta!
                string minhaRestApi = "https://localhost:7139/api/Destinations";
                // NOTA: Mudei para /api/Destinations ou /api/Reservas conforme o teu Controller

                try
                {
                    // 2. BUSCAR DADOS (GET)
                    // Adicionamos um User-Agent para a API não bloquear o pedido (boa prática)
                    client.DefaultRequestHeaders.Add("User-Agent", "TravelSoapService/1.0");

                    var response = await client.GetAsync(urlHotel);

                    if (!response.IsSuccessStatusCode)
                    {
                        return $"Erro na API Externa: {response.StatusCode} - Verifique a API Key.";
                    }

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // A API retorna um Array, usamos JArray para ler
                    JArray dados = JArray.Parse(jsonResponse);

                    int contador = 0;

                    // 3. PROCESSAR E ENVIAR
                    foreach (var item in dados)
                    {
                        // A MakCorps põe metadados no fim do array que não têm 'hotelId'
                        if (item["hotelId"] == null) continue;

                        string nomeHotel = item["name"]?.ToString() ?? "Hotel Desconhecido";

                        // --- AQUI CRIAMOS O JSON PARA MANDAR PARA O TEU BACKEND ---
                        // Ajuste estes campos conforme o teu DTO no DestinationsController ou ReservationsController
                        var jsonParaEnviar = new JObject();

                        // Exemplo: Vamos criar um Destino novo baseado no Hotel
                        jsonParaEnviar["city"] = "New York";
                        jsonParaEnviar["country"] = "USA";
                        jsonParaEnviar["description"] = $"Hotel importado: {nomeHotel}";
                        jsonParaEnviar["imageUrl"] = "https://example.com/hotel.jpg"; // Placeholder
                        jsonParaEnviar["countryCode"] = "US"; // Para os feriados

                        var content = new StringContent(jsonParaEnviar.ToString(), Encoding.UTF8, "application/json");

                        // Envia para o teu Backend (REST)
                        var postResponse = await client.PostAsync(minhaRestApi, content);

                        if (postResponse.IsSuccessStatusCode) contador++;
                    }

                    return $"Sucesso: {contador} hotéis importados de Nova Iorque.";
                }
                catch (Exception ex)
                {
                    return $"Erro Crítico: {ex.Message}";
                }
            }
        }
    }
}