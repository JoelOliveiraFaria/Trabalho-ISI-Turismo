using CalendarService.Services;
using Microsoft.AspNetCore.Mvc;
using CalendarService.Models;

namespace CalendarService.Controllers
{
    public class CalendarController : ControllerBase
    {
        private readonly GoogleCalendarService _googleService;

        public CalendarController(GoogleCalendarService googleService)
        {
            _googleService = googleService;
        }

        [HttpPost("check")]
        public async Task<IActionResult> CheckConflicts([FromBody] DateRangeRequest request)
        {
            var conflicts = await _googleService.CheckConflictsAsync(request.Start, request.End);
            return Ok(conflicts);
        }
    }
}
