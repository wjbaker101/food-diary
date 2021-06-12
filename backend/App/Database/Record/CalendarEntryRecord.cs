using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using WJBCommon.Lib.Data.Type;

namespace FoodDiaryApi.App.Database.Record
{
    public class CalendarEntryRecord : DatabaseRecord
    {
        public virtual Guid Reference { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime At { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual int DaySpan { get; set; }
        public virtual IList<CalendarEntryExclusionRecord> Exclusions { get; set; }
    }

    public sealed class CalendarEntryRecordMap : ClassMap<CalendarEntryRecord>
    {
        public CalendarEntryRecordMap()
        {
            Schema("calendar");
            Table("entry");
            Id(x => x.Id, "id").GeneratedBy.Sequence("entry_id_seq");
            Map(x => x.Reference, "reference");
            Map(x => x.Title, "title");
            Map(x => x.Description, "description");
            Map(x => x.At, "at");
            Map(x => x.CreatedAt, "created_at");
            Map(x => x.DaySpan, "day_span");
            HasMany(x => x.Exclusions);
        }
    }
}