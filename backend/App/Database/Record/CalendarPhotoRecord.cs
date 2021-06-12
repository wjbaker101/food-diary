using FluentNHibernate.Mapping;
using System;
using WJBCommon.Lib.Data.Type;

namespace FoodDiaryApi.App.Database.Record
{
    public class CalendarPhotoRecord : DatabaseRecord
    {
        public virtual Guid Reference { get; set; }
        public virtual string Description { get; set; }
        public virtual string PhotoUrl { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual DateTime UploadedAt { get; set; }
    }

    public sealed class CalendarPhotoRecordMap : ClassMap<CalendarPhotoRecord>
    {
        public CalendarPhotoRecordMap()
        {
            Schema("calendar");
            Table("photo");
            Id(x => x.Id, "id").GeneratedBy.Sequence("photo_id_seq");
            Map(x => x.Reference, "reference");
            Map(x => x.Description, "description");
            Map(x => x.PhotoUrl, "photo_url");
            Map(x => x.Date, "date");
            Map(x => x.UploadedAt, "uploaded_at");
        }
    }
}