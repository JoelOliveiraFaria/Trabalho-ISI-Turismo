/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: GoogleCalendarService.cs
 * Descrição: Serviço de integração com a Google Calendar API.
 * Responsável por autenticar e verificar conflitos de agenda.
 * ===================================================================================
 */

using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace CalendarService.Services
{
    /// <summary>
    /// Serviço que implementa a lógica de comunicação com a Google Calendar API v3.
    /// </summary>
    public class GoogleCalendarService
    {
        private readonly string _calendarId = "joel.o.faria25@gmail.com";

        /// <summary>
        /// Verifica eventos existentes num intervalo de tempo.
        /// Em caso de erro, retorna a mensagem de exceção na lista.
        /// </summary>
        /// <param name="start">Data de início.</param>
        /// <param name="end">Data de fim.</param>
        /// <returns>Lista de conflitos ou mensagens de erro.</returns>
        public async Task<List<string>> CheckConflictsAsync(DateTime start, DateTime end)
        {
            var conflicts = new List<string>();

            try
            {
                if (!File.Exists("google-key.json"))
                {
                    return new List<string> { "Google API key file not found." };
                }

                GoogleCredential credential;

                string jsonContent = File.ReadAllText("google-key.json");

                var serviceAccount = CredentialFactory.FromJson<ServiceAccountCredential>(jsonContent);

                credential = serviceAccount.ToGoogleCredential()
                    .CreateScoped(Google.Apis.Calendar.v3.CalendarService.Scope.CalendarEventsReadonly);

                var service = new Google.Apis.Calendar.v3.CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "CalendarService",
                });

                var request = service.Events.List(_calendarId);
                request.TimeMinDateTimeOffset = start;
                request.TimeMaxDateTimeOffset = end;
                request.SingleEvents = true;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                var events = await request.ExecuteAsync();

                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items)
                    {
                        string dataEvento = eventItem.Start.DateTimeRaw ?? eventItem.Start.Date;
                        conflicts.Add($"Conflito: {eventItem.Summary} ({dataEvento})");
                    }
                }
            }
            catch (Exception ex)
            {
                conflicts.Add(ex.Message);
            }
            return conflicts;
        }
    }
}