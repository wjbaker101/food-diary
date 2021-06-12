using System;
using FoodDiaryApi.App._Api.Calendar.Entry.Type;
using Microsoft.AspNetCore.Mvc;
using WJBCommon.Lib.Api.Controller;

namespace FoodDiaryApi.App._Api.Calendar.Entry
{
    [Route("api/calendar")]
    public sealed class CalendarEntryController : ApiController
    {
        private readonly ICalendarEntryService _calendarEntryService;

        public CalendarEntryController(ICalendarEntryService calendarEntryService)
        {
            _calendarEntryService = calendarEntryService;
        }

        [HttpGet]
        [Route("entries/{startAt}/{endAt}")]
        public IActionResult GetEntries([FromRoute] DateTime startAt, [FromRoute] DateTime endAt)
        {
            return ApiResult(_calendarEntryService.GetEntries(startAt, endAt));
        }

        [HttpPost]
        [Route("entry")]
        public IActionResult AddEntry([FromBody] AddEntryRequest request)
        {
            return ApiCreatedResult(_calendarEntryService.AddEntry(request));
        }

        [HttpDelete]
        [Route("entry/{reference:guid}")]
        public IActionResult RemoveEntry([FromRoute] Guid reference)
        {
            _calendarEntryService.RemoveEntry(reference);
            
            return ApiNoContentResult();
        }

        [HttpPost]
        [Route("entry/{entryReference:guid}/exclude/{date}")]
        public IActionResult AddEntryExclusion(Guid entryReference, DateTime date)
        {
            _calendarEntryService.AddEntryExclusion(entryReference, date);

            return ApiCreatedResult(true);
        }

        [HttpDelete]
        [Route("entry/{entryReference:guid}/exclude/{date}")]
        public IActionResult RemoveEntryExclusion(Guid entryReference, DateTime date)
        {
            _calendarEntryService.RemoveEntryExclusion(entryReference, date);

            return ApiNoContentResult();
        }
    }
}