using FoodDiaryApi.App.Database.Record;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WJBCommon.Lib.Data;

namespace FoodDiaryApi.App.Database.Repository
{
    public interface ICalendarEntryRepository : IRepository<CalendarEntryRecord>
    {
        List<CalendarEntryRecord> GetBetweenStartAtAndEndAt(DateTime startAt, DateTime endAt);
        CalendarEntryRecord GetByReference(Guid reference);
        CalendarEntryRecord GetByReferenceWithExclusions(Guid reference);
    }

    public sealed class CalendarEntryRepository : Repository<CalendarEntryRecord>, ICalendarEntryRepository
    {
        public CalendarEntryRepository(ISession session) : base(session)
        {
        }

        public List<CalendarEntryRecord> GetBetweenStartAtAndEndAt(DateTime startAt, DateTime endAt)
        {
            return Session
                .Query<CalendarEntryRecord>()
                .Fetch(x => x.Exclusions)
                .Where(x => x.At >= startAt && x.At < endAt)
                .ToList();
        }

        public CalendarEntryRecord GetByReference(Guid reference)
        {
            return Session
                .Query<CalendarEntryRecord>()
                .SingleOrDefault(x => x.Reference == reference);
        }

        public CalendarEntryRecord GetByReferenceWithExclusions(Guid reference)
        {
            return Session
                .Query<CalendarEntryRecord>()
                .Fetch(x => x.Exclusions)
                .SingleOrDefault(x => x.Reference == reference);
        }
    }
}