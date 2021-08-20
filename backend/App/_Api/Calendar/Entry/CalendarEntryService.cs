using FoodDiaryApi.App._Api.Calendar.Entry.Type;
using FoodDiaryApi.App.Database.Record;
using System;
using System.Linq;
using WJBCommon.Lib.Api.Exception;
using WJBCommon.Lib.Data;

namespace FoodDiaryApi.App._Api.Calendar.Entry
{
    public interface ICalendarEntryService
    {
        GetEntriesResponse GetEntries(DateTime startAt, DateTime endAt);
        AddEntryResponse AddEntry(AddEntryRequest request);
        void RemoveEntry(Guid reference);
        void AddEntryExclusion(Guid entryReference, DateTime date);
        void RemoveEntryExclusion(Guid entryReference, DateTime date);
    }

    public sealed class CalendarEntryService : ICalendarEntryService
    {
        private readonly IUnitOfWorkFactory<ICalendarUnitOfWork> _unitOfWorkFactory;

        public CalendarEntryService(IUnitOfWorkFactory<ICalendarUnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public GetEntriesResponse GetEntries(DateTime startAt, DateTime endAt)
        {
            using var unitOfWork = _unitOfWorkFactory.Create();

            var entries = unitOfWork.Entries.GetBetweenStartAtAndEndAt(startAt.ToLocalTime(), endAt.ToLocalTime());

            return new GetEntriesResponse
            {
                Entries = entries.ConvertAll(x => new GetEntriesResponse.Entry
                {
                    Reference = x.Reference,
                    Title = x.Title,
                    Description = x.Description,
                    At = x.At,
                    CreatedAt = x.CreatedAt,
                    DaySpan = x.DaySpan,
                    Exclusions = x.Exclusions.Select(y => new GetEntriesResponse.Exclusion
                    {
                        Date = y.Date
                    }).ToList()
                })
            };
        }

        public AddEntryResponse AddEntry(AddEntryRequest request)
        {
            using var unitOfWork = _unitOfWorkFactory.Create();

            var entry = new CalendarEntryRecord
            {
                Reference = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                At = request.At.ToLocalTime(),
                CreatedAt = DateTime.Now,
                DaySpan = request.DaySpan
            };

            unitOfWork.Entries.Save(entry);

            unitOfWork.Commit();

            return new AddEntryResponse
            {
                Reference = entry.Reference,
                Title = entry.Title,
                Description = entry.Description,
                At = entry.At,
                CreatedAt = entry.CreatedAt,
                DaySpan = entry.DaySpan
            };
        }

        public void RemoveEntry(Guid reference)
        {
            using var unitOfWork = _unitOfWorkFactory.Create();

            var entry = unitOfWork.Entries.GetByReference(reference);
            if (entry == null)
                throw new ApiNotFoundException();

            unitOfWork.Entries.Delete(entry);

            unitOfWork.Commit();
        }

        public void AddEntryExclusion(Guid entryReference, DateTime date)
        {
            using var unitOfWork = _unitOfWorkFactory.Create();

            var entry = unitOfWork.Entries.GetByReferenceWithExclusions(entryReference);
            if (entry == null)
                throw new ApiNotFoundException();

            var dateInLocalTime = date.ToLocalTime();

            if (entry.Exclusions.Any(x => x.Date == dateInLocalTime))
                throw new ApiBadRequestException("The given date has already been excluded for this calendar entry. Choose another date and try again or if you happy with the current state, ignore this message.");

            unitOfWork.EntryExclusions.Save(new CalendarEntryExclusionRecord
            {
                Entry = entry,
                Date = dateInLocalTime
            });

            unitOfWork.Commit();
        }

        public void RemoveEntryExclusion(Guid entryReference, DateTime date)
        {
            using var unitOfWork = _unitOfWorkFactory.Create();

            var entry = unitOfWork.Entries.GetByReferenceWithExclusions(entryReference);
            if (entry == null)
                throw new ApiNotFoundException();

            var exclusion = unitOfWork.EntryExclusions.Get(entry, date);
            if (exclusion == null)
                throw new ApiNotFoundException();

            unitOfWork.EntryExclusions.Delete(exclusion);

            unitOfWork.Commit();
        }
    }
}