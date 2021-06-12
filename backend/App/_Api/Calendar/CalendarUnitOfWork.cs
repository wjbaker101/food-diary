using FoodDiaryApi.App.Database.Repository;
using WJBCommon.Lib.Data;

namespace FoodDiaryApi.App._Api.Calendar
{
    public interface ICalendarUnitOfWork : IUnitOfWork
    {
        public ICalendarEntryRepository Entries { get; }
        public ICalendarPhotoRepository Photos { get; }
        public ICalendarEntryExclusionRepository EntryExclusions { get; }
    }

    public sealed class CalendarUnitOfWork : UnitOfWork, ICalendarUnitOfWork
    {
        public ICalendarEntryRepository Entries { get; }
        public ICalendarPhotoRepository Photos { get; }
        public ICalendarEntryExclusionRepository EntryExclusions { get; }

        public CalendarUnitOfWork(IApiDatabase database) : base(database)
        {
            Entries = new CalendarEntryRepository(Session);
            Photos = new CalendarPhotoRepository(Session);
            EntryExclusions = new CalendarEntryExclusionRepository(Session);
        }
    }
}