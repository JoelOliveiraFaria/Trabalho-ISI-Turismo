using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace CalendarService.Services
{
    public class GoogleCalendarService
    {
        private readonly string _calendarId = "joel.o.faria25@gmail.com";

        public async Task<List<string>> CheckConflictsAsync(DateTime start, DateTime end)
        {
            var conflicts = new List<string>();

            try
            {
                if(!File.Exists("google-key.json"))
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

                if(events.Items != null && events.Items.Count > 0)
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
