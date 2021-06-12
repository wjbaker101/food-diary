using FoodDiaryApi.App.Database.Record;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using WJBCommon.Lib.Data;

namespace FoodDiaryApi.App.Database.Repository
{
    public interface ICookbookRecipeRepository : IRepository<CookbookRecipeRecord>
    {
        List<CookbookRecipeRecord> GetAll();
        CookbookRecipeRecord GetByReference(Guid reference);
    }

    public sealed class CookbookRecipeRepository : Repository<CookbookRecipeRecord>, ICookbookRecipeRepository
    {
        public CookbookRecipeRepository(ISession session) : base(session)
        {
        }

        public List<CookbookRecipeRecord> GetAll()
        {
            return Session
                .Query<CookbookRecipeRecord>()
                .ToList();
        }

        public CookbookRecipeRecord GetByReference(Guid reference)
        {
            return Session
                .Query<CookbookRecipeRecord>()
                .SingleOrDefault(x => x.Reference == reference);
        }
    }
}