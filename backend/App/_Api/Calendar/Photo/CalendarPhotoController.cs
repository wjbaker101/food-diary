using FoodDiaryApi.App._Api.Calendar.Photo.Type;
using Microsoft.AspNetCore.Mvc;
using WJBCommon.Lib.Api.Controller;

namespace FoodDiaryApi.App._Api.Calendar.Photo
{
    [Route("api/calendar")]
    public sealed class CalendarPhotoController : ApiController
    {
        private readonly ICalendarPhotoService _calendarPhotoService;

        public CalendarPhotoController(ICalendarPhotoService calendarPhotoService)
        {
            _calendarPhotoService = calendarPhotoService;
        }

        [HttpPost]
        [Route("photo")]
        public IActionResult UploadPhoto([FromForm] UploadPhotoRequest request)
        {
            var response = _calendarPhotoService.UploadPhoto(request);

            return ApiCreatedResult(response);
        }
    }
}