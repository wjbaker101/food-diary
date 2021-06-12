using FluentNHibernate.Mapping;
using System;
using WJBCommon.Lib.Data.Type;

namespace FoodDiaryApi.App.Database.Record
{
    public class CalendarEntryExclusionRecord : DatabaseRecord
    {
        public virtual CalendarEntryRecord Entry { get; set; }
        public virtual DateTime Date { get; set; }
    }

    public sealed class CalendarEntryExclusionRecordMap : ClassMap<CalendarEntryExclusionRecord>
    {
        public CalendarEntryExclusionRecordMap()
        {
            Schema("calendar");
            Table("entry_exclusion");
            Id(x => x.Id, "id").GeneratedBy.Sequence("entry_exclusion_id_seq");
            References(x => x.Entry);
            Map(x => x.Date, "date");
        }
    }
}