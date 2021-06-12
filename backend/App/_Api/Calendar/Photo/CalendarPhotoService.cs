using FoodDiaryApi.App._Api.Calendar.Photo.Type;
using FoodDiaryApi.App.Client.Imgur;
using FoodDiaryApi.App.Database.Record;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using WJBCommon.Lib.Api.Exception;
using WJBCommon.Lib.Data;
using Size = SixLabors.ImageSharp.Size;

namespace FoodDiaryApi.App._Api.Calendar.Photo
{
    public interface ICalendarPhotoService
    {
        UploadPhotoResponse UploadPhoto(UploadPhotoRequest request);
    }

    public sealed class CalendarPhotoService : ICalendarPhotoService
    {
        private const int MaxPhotoSize = 1024 * 1024 * 10;

        private readonly IUnitOfWorkFactory<ICalendarUnitOfWork> _unitOfWorkFactory;
        private readonly IImgurClient _imgurClient;

        public CalendarPhotoService(IUnitOfWorkFactory<ICalendarUnitOfWork> unitOfWorkFactory, IImgurClient imgurClient)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _imgurClient = imgurClient;
        }

        public UploadPhotoResponse UploadPhoto(UploadPhotoRequest request)
        {
            if (request.Photo == null || request.Photo.Length == 0)
                throw new ApiBadRequestException("A valid photo was not provided. Please check the photo you are uploading and try again.");

            if (request.Photo.Length > MaxPhotoSize)
                throw new ApiBadRequestException("The provided photo has a file size that was too large. Please crop the photo or choose another one and try again.");

            using var photoImage = Image.Load(request.Photo.OpenReadStream());
            
            photoImage.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(1500, 1500),
                Mode = ResizeMode.Min
            }));

            var photoUrl = _imgurClient.UploadImage(photoImage.ToBase64String(PngFormat.Instance));

            using var unitOfWork = _unitOfWorkFactory.Create();

            var photo = new CalendarPhotoRecord
            {
                Reference = Guid.NewGuid(),
                Description = request.Description,
                PhotoUrl = photoUrl,
                Date = request.Date.ToLocalTime(),
                UploadedAt = DateTime.Now
            };

            unitOfWork.Photos.Save(photo);

            unitOfWork.Commit();

            return new UploadPhotoResponse
            {
                Reference = photo.Reference,
                Description = photo.Description,
                PhotoUrl = photo.PhotoUrl,
                Date = photo.Date,
                UploadedAt = photo.UploadedAt
            };
        }
    }
}