using Microsoft.AspNetCore.Http;
using System;

namespace FoodDiaryApi.App._Api.Calendar.Photo.Type
{
    public sealed class UploadPhotoRequest
    {
        public string Description { get; init; }
        public IFormFile Photo { get; init; }
        public DateTime Date { get; init; }
    }

    public sealed class UploadPhotoResponse
    {
        public Guid Reference { get; init; }
        public string Description { get; init; }
        public string PhotoUrl { get; init; }
        public DateTime Date { get; init; }
        public DateTime UploadedAt { get; init; }
    }
}