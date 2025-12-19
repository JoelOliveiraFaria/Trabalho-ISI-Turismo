namespace TravelAPI.Interfaces
{
    public interface ICalendarService
    {
        Task<List<string>> CheckConflictsAsync(DateTime start, DateTime end);
    }
}
