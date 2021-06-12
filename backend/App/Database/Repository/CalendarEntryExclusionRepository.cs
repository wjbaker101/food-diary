using FoodDiaryApi.App.Database.Record;
using NHibernate;
using System;
using System.Linq;
using WJBCommon.Lib.Data;

namespace FoodDiaryApi.App.Database.Repository
{
    public interface ICalendarEntryExclusionRepository : IRepository<CalendarEntryExclusionRecord>
    {
        CalendarEntryExclusionRecord Get(CalendarEntryRecord entry, DateTime date);
    }

    public sealed class CalendarEntryExclusionRepository : Repository<CalendarEntryExclusionRecord>, ICalendarEntryExclusionRepository
    {
        public CalendarEntryExclusionRepository(ISession session) : base(session)
        {
        }

        public CalendarEntryExclusionRecord Get(CalendarEntryRecord entry, DateTime date)
        {
            return Session
                .Query<CalendarEntryExclusionRecord>()
                .SingleOrDefault(x => x.Entry == entry && x.Date == date);
        }
    }
}