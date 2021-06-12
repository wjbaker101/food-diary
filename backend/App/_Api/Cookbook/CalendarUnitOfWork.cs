using FoodDiaryApi.App.Database.Repository;
using WJBCommon.Lib.Data;

namespace FoodDiaryApi.App._Api.Cookbook
{
    public interface ICookbookUnitOfWork : IUnitOfWork
    {
        public ICookbookRecipeRepository Recipes { get; }
    }

    public sealed class CookbookUnitOfWork : UnitOfWork, ICookbookUnitOfWork
    {
        public ICookbookRecipeRepository Recipes { get; }

        public CookbookUnitOfWork(IApiDatabase database) : base(database)
        {
            Recipes = new CookbookRecipeRepository(Session);
        }
    }
}