using FoodDiaryApi.App.Database.Record;
using NHibernate;
using WJBCommon.Lib.Data;

namespace FoodDiaryApi.App.Database.Repository
{
    public interface ICalendarPhotoRepository : IRepository<CalendarPhotoRecord>
    { 
    }

    public class CalendarPhotoRepository : Repository<CalendarPhotoRecord>, ICalendarPhotoRepository
    {
        public CalendarPhotoRepository(ISession session) : base(session)
        {
        }
    }
}